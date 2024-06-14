/********************************************************************************************
 * Project Name - Utitlities
 * Description  - Validate token adn authorization 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.40        23-Sep-2018   Manoj          Created
 *2.60        14-Mar-2019   Jagan Mohan    Implemented Roles Authorization for Form Access : ValidateFormAccess
 *2.80        14-Apr-2020   Faizan         Customer Registration Changes
 *2.110       09-Feb-2021   Girish Kundar  Modified: Added SessionId in SecurityToken table
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace Semnox.Core.Utilities
{
    public class SecurityTokenHandler
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM SecurityTokens AS st ";

        private static readonly Dictionary<SecurityTokenDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<SecurityTokenDTO.SearchByParameters, string>
            {
                {SecurityTokenDTO.SearchByParameters.TOKENID, "TOKENID"},
                {SecurityTokenDTO.SearchByParameters.TOKEN, "Token"},
                {SecurityTokenDTO.SearchByParameters.OBJECT, "Object"},
                {SecurityTokenDTO.SearchByParameters.OBJECT_GUID, "ObjectGuid"},
                {SecurityTokenDTO.SearchByParameters.START_TIME, "StartTime"},
                {SecurityTokenDTO.SearchByParameters.EXPIRY_TIME, "ExpiryTime"},
				{SecurityTokenDTO.SearchByParameters.MASTER_ENTITY_ID, "MasterEntityId"},
				{SecurityTokenDTO.SearchByParameters.USER_SESSION_ID, "UserSessionId"},
				{SecurityTokenDTO.SearchByParameters.INVALID_ATTEMPTS, "InValidAttempts"},
				{SecurityTokenDTO.SearchByParameters.ACTIVE_FLAG, "IsActive"},
                {SecurityTokenDTO.SearchByParameters.SITE_ID, "site_id"},
                {SecurityTokenDTO.SearchByParameters.IS_EXPIRED, "ExpiryTime"}
             };

        /// <summary>
        /// Default constructor of SecurityTokenHandler class
        /// </summary>
        public SecurityTokenHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }


        private List<SqlParameter> GetSQLParameters(SecurityTokenDTO securityTokenDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(securityTokenDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@tokenId", securityTokenDTO.TokenId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@token", securityTokenDTO.Token));
            parameters.Add(dataAccessHandler.GetSQLParameter("@tableObject", securityTokenDTO.TableObject));
            parameters.Add(dataAccessHandler.GetSQLParameter("@objectGuid", securityTokenDTO.ObjectGuid));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", securityTokenDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@invalidAttempts", securityTokenDTO.InvalidAttempts,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UserSessionId", securityTokenDTO.UserSessionId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@expiryTime", securityTokenDTO.ExpiryTime == DateTime.MinValue? DBNull.Value : (object)securityTokenDTO.ExpiryTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", securityTokenDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            //parameters.Add(dataAccessHandler.GetSQLParameter("@SynchStatus", securityTokenDTO.SynchStatus));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the SecurityToken record to the database
        /// </summary>
        /// <param name="SecurityTokenDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        ///<returns>Returns inserted record id</returns>
        public SecurityTokenDTO InsertSecurityTokenDTO(SecurityTokenDTO securityTokenDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(securityTokenDTO, loginId, siteId);
            string query = @"insert into SecurityTokens
                                                            ( 
															Token,
                                                            Object,
                                                            ObjectGuid,
                                                            StartTime,
                                                            ExpiryTime,
                                                            LastActivityTime,
                                                            IsActive,
                                                            CreatedBy,
                                                            CreationDate,
                                                            LastUpdatedBy,
                                                            LastUpdatedDate,
                                                            site_id,
                                                            MasterEntityId,
                                                            Guid,
                                                            UserSessionId
                                                         )
                                                       values
                                                         ( 
															@token,
                                                            @tableObject,
                                                            @objectGuid,
                                                            GETDATE(),
                                                            @expiryTime,
                                                            GETDATE(),
                                                            @isActive,
                                                            @createdBy,
                                                            GETDATE(),
                                                            @lastUpdatedBy,
                                                            GETDATE(),
                                                            @siteId,
                                                            @masterEntityId,
                                                            NewID(),
                                                            @UserSessionId
                                                          )SELECT* FROM SecurityTokens WHERE TokenId = scope_identity()";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(securityTokenDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshSecurityTokenDTO(securityTokenDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(securityTokenDTO);
            return securityTokenDTO;
    }

    private void RefreshSecurityTokenDTO(SecurityTokenDTO securityTokenDTO, DataTable dt)
    {
        log.LogMethodEntry(securityTokenDTO, dt);
        if (dt.Rows.Count > 0)
        {
            DataRow dataRow = dt.Rows[0];
            securityTokenDTO.TokenId = Convert.ToInt32(dt.Rows[0]["TokenId"]);
            securityTokenDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
            securityTokenDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
            securityTokenDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
            securityTokenDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
            securityTokenDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
            securityTokenDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
        }
        log.LogMethodExit();
    }
    /// <summary>
    /// Updates the SecurityToken record to the database
    /// </summary>
    /// <param name="securityTokenDTO"></param>
    /// <param name="loginId"></param>
    /// <param name="siteId">Returns # of rows updated</param>
    /// <returns></returns>
    public SecurityTokenDTO UpdateSecurityTokenDTO(SecurityTokenDTO securityTokenDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(securityTokenDTO, loginId, siteId);
            string query = @"update SecurityTokens
                                                         set
															Token= @token,
															--ExpiryTime = @expiryTime,
                                                            LastActivityTime = GETDATE(),
                                                            InvalidAttempts = @invalidAttempts,
                                                            IsActive = @isActive,
                                                            LastUpdatedBy = @lastUpdatedBy,
                                                            LastUpdatedDate = GETDATE(),
                                                            UserSessionId = @UserSessionId,
                                                           -- site_id = @siteId,
                                                            MasterEntityId = @masterEntityId
                                                         --   SynchStatus = @synchStatus
                                                          where 
                                                            TokenId = @tokenId
                                        SELECT * FROM SecurityTokens WHERE TokenId = @tokenId ";
 
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(securityTokenDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshSecurityTokenDTO(securityTokenDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(securityTokenDTO);
            return securityTokenDTO;
        }



        /// <summary>
        ///  Converts the Data row object to SecurityTokenDTO class type
        /// </summary>
        /// <param name="SecurityTokenDTODataRow"></param>
        /// <returns>Returns SecurityTokenDTO</returns>
        private SecurityTokenDTO GetSecurityTokenDTO(DataRow SecurityTokenDTODataRow)
        {
            log.LogMethodEntry();
            SecurityTokenDTO SecurityTokenDTO = new SecurityTokenDTO(
                                 SecurityTokenDTODataRow["TokenId"] == DBNull.Value ? -1 : Convert.ToInt32(SecurityTokenDTODataRow["TokenId"]),
                                 SecurityTokenDTODataRow["Token"].ToString(),
                                 SecurityTokenDTODataRow["Object"].ToString(),
                                 SecurityTokenDTODataRow["ObjectGuid"].ToString(),
                                 SecurityTokenDTODataRow["StartTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(SecurityTokenDTODataRow["StartTime"]),
                                 SecurityTokenDTODataRow["ExpiryTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(SecurityTokenDTODataRow["ExpiryTime"]),
                                 SecurityTokenDTODataRow["LastActivityTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(SecurityTokenDTODataRow["LastActivityTime"]),
                                 SecurityTokenDTODataRow["InvalidAttempts"] == DBNull.Value ? -1 : Convert.ToInt32(SecurityTokenDTODataRow["InvalidAttempts"]),
                                 SecurityTokenDTODataRow["IsActive"].ToString(),
                                 SecurityTokenDTODataRow["CreatedBy"].ToString(),
                                 SecurityTokenDTODataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(SecurityTokenDTODataRow["CreationDate"]),
                                 SecurityTokenDTODataRow["LastUpdatedBy"].ToString(),
                                 SecurityTokenDTODataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(SecurityTokenDTODataRow["LastUpdatedDate"]),
                                 SecurityTokenDTODataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(SecurityTokenDTODataRow["Site_id"]),
                                 SecurityTokenDTODataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(SecurityTokenDTODataRow["MasterEntityId"]),
                                 SecurityTokenDTODataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(SecurityTokenDTODataRow["SynchStatus"]),
                                 SecurityTokenDTODataRow["Guid"].ToString(),
                                 string.Empty,
                                 SecurityTokenDTODataRow["UserSessionId"].ToString()
                                 );
            log.LogMethodExit();
            return SecurityTokenDTO;
        }

        /// <summary>
        /// Gets the SecurityTokensDTO data of passed tokenId
        /// </summary>
        /// <param name="tokenId"></param>
        /// <returns>Returns SecurityTokensDTO</returns>
        public SecurityTokenDTO GetSecurityTokenDTO(int tokenId)
        {
            log.LogMethodEntry();
            SecurityTokenDTO SecurityTokenDTO = null;
            string selectSecurityTokenDTOQuery = SELECT_QUERY + " where st.TokenId = @tokenId";
            SqlParameter[] selectSecurityTokenDTOParameters = new SqlParameter[1];
            selectSecurityTokenDTOParameters[0] = new SqlParameter("@tokenId", tokenId);
            DataTable selectedSecurityTokenDTO = dataAccessHandler.executeSelectQuery(selectSecurityTokenDTOQuery, selectSecurityTokenDTOParameters,sqlTransaction);
            if (selectedSecurityTokenDTO.Rows.Count > 0)
            {
                DataRow SecurityTokenRow = selectedSecurityTokenDTO.Rows[0];
                SecurityTokenDTO = GetSecurityTokenDTO(SecurityTokenRow);
            }

            log.LogMethodExit();
            return SecurityTokenDTO;
        }

        /// <summary>
        /// Get the Form access allowed or not based on the roleId and formName.
        /// </summary>
        /// <param name="formName"></param>
        /// <param name="roleId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public bool ValidateFormAccess(string formName, string roleId, string siteId)
        {
            log.LogMethodEntry();
            bool access = false;
            string selectQuery = @"select access_allowed from ManagementFormAccess
                                   where role_id = @roleId and form_name = @formName and (site_id = @siteId or @siteId = -1) and isnull(IsActive,1)=1";
            List<SqlParameter> selectQueryParameters = new List<SqlParameter>();
            selectQueryParameters.Add(new SqlParameter("@roleId", Convert.ToInt32(roleId)));
            selectQueryParameters.Add(new SqlParameter("@formName", formName));
            selectQueryParameters.Add(new SqlParameter("@siteId", Convert.ToInt32(siteId)));
            DataTable selectedFormAccess = dataAccessHandler.executeSelectQuery(selectQuery, selectQueryParameters.ToArray(),sqlTransaction);
            if (selectedFormAccess.Rows.Count > 0)
            {
                DataRow formAccessRow = selectedFormAccess.Rows[0];
                string formAccess = formAccessRow["access_allowed"].ToString();
                if (formAccess == "Y")
                {
                    access = true;
                }
            }
            log.LogMethodExit();
            return access;
        }

        /// <summary>
        /// Get the userRoleId based on the loginId and siteId
        /// </summary>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns>RoleId</returns>
        public string GetUserRoleId(string loginId, string siteId)
        {
            log.LogMethodEntry();
            string roleId = string.Empty;
            string selectQuery = @"select role_id from users
                                   where loginid = @loginid and (site_id = @siteId or @siteId = -1)";
            List<SqlParameter> selectQueryParameters = new List<SqlParameter>();

            selectQueryParameters.Add(new SqlParameter("@loginid", loginId));
            selectQueryParameters.Add(new SqlParameter("@siteId", Convert.ToInt32(siteId)));
            DataTable selectedUserRole = dataAccessHandler.executeSelectQuery(selectQuery, selectQueryParameters.ToArray(),sqlTransaction);
            if (selectedUserRole.Rows.Count > 0)
            {
                DataRow formAccessRow = selectedUserRole.Rows[0];
                roleId = formAccessRow["role_id"].ToString();
            }
            log.LogMethodExit(roleId);
            return roleId;
        }
        /// <summary>
        /// Gets the SecurityTokensDTO data of passed tokenId
        /// </summary>
        /// <param name="tokenId"></param>
        /// <returns>Returns SecurityTokensDTO</returns>
        public SecurityTokenDTO GetSecurityTokenDTO(string tokenValue,string userSessionId =null)
        {
            log.LogMethodEntry(tokenValue ,userSessionId);
            SecurityTokenDTO SecurityTokenDTO = null;
            string selectSecurityTokenDTOQuery = SELECT_QUERY + "    where st.Token = @tokenValue";
            //SqlParameter[] selectSecurityTokenDTOParameters = new SqlParameter[2];
            List<SqlParameter> selectSecurityTokenDTOParameters = new List<SqlParameter>();
            //selectSecurityTokenDTOParameters[0] = new SqlParameter("@tokenValue", tokenValue);
            selectSecurityTokenDTOParameters.Add(dataAccessHandler.GetSQLParameter("@tokenValue", tokenValue));
            if (string.IsNullOrWhiteSpace(userSessionId) == false)
            {
                selectSecurityTokenDTOQuery += @" and st.UserSessionId= @userSessionId ";
                //selectSecurityTokenDTOParameters[1] = new SqlParameter("@userSessionId", userSessionId);
                selectSecurityTokenDTOParameters.Add(dataAccessHandler.GetSQLParameter("@userSessionId", userSessionId));
            }
            DataTable selectedSecurityTokenDTO = dataAccessHandler.executeSelectQuery(selectSecurityTokenDTOQuery, selectSecurityTokenDTOParameters.ToArray(),sqlTransaction);
            if (selectedSecurityTokenDTO.Rows.Count > 0)
            {
                DataRow SecurityTokenRow = selectedSecurityTokenDTO.Rows[0];
                SecurityTokenDTO = GetSecurityTokenDTO(SecurityTokenRow);
            }
            log.LogMethodExit();
            return SecurityTokenDTO;
        }

        /// <summary>
        /// Gets the SecurityTokenDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns>Returns the list of SecurityTokensDTO matching the search criteria</returns>
        public List<SecurityTokenDTO> GetSecurityTokenDTOList(List<KeyValuePair<SecurityTokenDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry();
            string selectSecurityTokenDTOQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                int count = 0;
                StringBuilder query = new StringBuilder(" where ");

                foreach (KeyValuePair<SecurityTokenDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        string joinOperartor = (count == 0) ? " " : " and ";
                        if (searchParameter.Key.Equals(SecurityTokenDTO.SearchByParameters.TOKENID)
                            || searchParameter.Key.Equals(SecurityTokenDTO.SearchByParameters.INVALID_ATTEMPTS)
                            || searchParameter.Key == SecurityTokenDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                                query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(SecurityTokenDTO.SearchByParameters.TOKEN)
                                  || searchParameter.Key.Equals(SecurityTokenDTO.SearchByParameters.OBJECT))
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), (searchParameter.Value)));
                        }
                     
                        else if (searchParameter.Key.Equals(SecurityTokenDTO.SearchByParameters.OBJECT_GUID)
                                  || searchParameter.Key.Equals(SecurityTokenDTO.SearchByParameters.USER_SESSION_ID))
                        {
                            query.Append(joinOperartor + "  CONVERT(varchar(200), " + DBSearchParameters[searchParameter.Key] + ") = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), (searchParameter.Value)));

                        }
                        else if (searchParameter.Key.Equals(SecurityTokenDTO.SearchByParameters.START_TIME))
                        {
                            query.Append(joinOperartor + "  ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key.Equals(SecurityTokenDTO.SearchByParameters.EXPIRY_TIME))
                        {
                            query.Append(joinOperartor + "  ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) < " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key.Equals(SecurityTokenDTO.SearchByParameters.IS_EXPIRED))
                        {
                            query.Append(joinOperartor + @" CASE WHEN ExpiryTime is null then 'N'
                                                                 WHEN ExpiryTime != null AND ExpiryTime < getdate() then 'Y'
                                                                 ELSE 'N' END = '" + searchParameter.Value + "' ");
                        }
						else if (searchParameter.Key.Equals(SecurityTokenDTO.SearchByParameters.ACTIVE_FLAG))
                        {
                            query.Append(joinOperartor + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key.Equals(SecurityTokenDTO.SearchByParameters.SITE_ID))
                        {
                            query.Append(joinOperartor + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }

                        count++;
                    }
                    else
                    {
                        log.Debug("Ends-GetSecurityTokenDTOList(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }

                if (searchParameters.Count > 0)
                    selectSecurityTokenDTOQuery = selectSecurityTokenDTOQuery + query;
            }

            DataTable SecurityTokenDTOData = dataAccessHandler.executeSelectQuery(selectSecurityTokenDTOQuery, parameters.ToArray(),sqlTransaction);
            List<SecurityTokenDTO> SecurityTokenDTOList = new List<SecurityTokenDTO>();
            if (SecurityTokenDTOData.Rows.Count > 0)
            {
                foreach (DataRow dataRow in SecurityTokenDTOData.Rows)
                {
                    SecurityTokenDTO SecurityTokenDTOObject = GetSecurityTokenDTO(dataRow);
                    SecurityTokenDTOList.Add(SecurityTokenDTOObject);
                }
              
            }
            log.LogMethodExit(SecurityTokenDTOList);
            return SecurityTokenDTOList;
        }
    }
}
