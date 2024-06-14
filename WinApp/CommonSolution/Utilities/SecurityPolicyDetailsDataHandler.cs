/********************************************************************************************
 * Project Name - Utilities
 * Description  - Get and Insert or update methods for Security policy details.
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70        24-Mar-2019   Jagan Mohana          Created 
              09-Apr-2019   Mushahid Faizan       Modified- GetSQLParameters() & GetSecurityPolicyDetailsDTO(), Insert/Update method.
                                                  Added DeleteSecurityPolicyDetails() method.
 *2.70.2        11-Dec-2019   Jinto Thomas          Removed siteid from update query
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;


namespace Semnox.Core.Utilities
{
    public class SecurityPolicyDetailsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;
        private static readonly Dictionary<SecurityPolicyDetailsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<SecurityPolicyDetailsDTO.SearchByParameters, string>
        {
            { SecurityPolicyDetailsDTO.SearchByParameters.POLICY_DETAIL_ID,"Id"},
            { SecurityPolicyDetailsDTO.SearchByParameters.POLICY_ID,"PolicyId"}
        };

        /// <summary>
        /// Default constructor of SecurityPolicyDetailsDataHandler class
        /// </summary>
        public SecurityPolicyDetailsDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            this.dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        ///  Converts the Data row object to SecurityPolicyDetailsDTO class type
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        /// Modified site_id & LastUpdatedDate column by Mushahid Faizan on 09-Apr-2019
        private SecurityPolicyDetailsDTO GetSecurityPolicyDetailsDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            SecurityPolicyDetailsDTO securityPolicyDetailsDTO = new SecurityPolicyDetailsDTO(Convert.ToInt32(dataRow["Id"]),
                                            dataRow["PolicyId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PolicyId"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? "" : Convert.ToString(dataRow["CreatedBy"]),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? "" : Convert.ToString(dataRow["LastUpdatedBy"]),
                                            dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["Guid"] == DBNull.Value ? "" : dataRow["Guid"].ToString(),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["PasswordChangeFrequency"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PasswordChangeFrequency"]),
                                            dataRow["PasswordMinLength"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PasswordMinLength"]),
                                            dataRow["PasswordMinAlphabets"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PasswordMinAlphabets"]),
                                            dataRow["PasswordMinNumbers"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PasswordMinNumbers"]),
                                            dataRow["PasswordMinSpecialChars"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PasswordMinSpecialChars"]),
                                            dataRow["RememberPasswordsCount"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["RememberPasswordsCount"]),
                                            dataRow["InvalidAttemptsBeforeLockout"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["InvalidAttemptsBeforeLockout"]),
                                            dataRow["LockoutDuration"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LockoutDuration"]),
                                            dataRow["UserSessionTimeout"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["UserSessionTimeout"]),
                                            dataRow["MaxUserInactivityDays"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MaxUserInactivityDays"]),
                                            dataRow["MaxDaysToLoginAfterUserCreation"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MaxDaysToLoginAfterUserCreation"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"])
                                            );
            log.LogMethodExit(securityPolicyDetailsDTO);
            return securityPolicyDetailsDTO;
        }

        /// <summary>
        /// Gets the SecurityPolicyDetailsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of SecurityPolicyDetailsDTO matching the search criteria</returns>
        public List<SecurityPolicyDetailsDTO> GetAllSecurityPolicyDetails(List<KeyValuePair<SecurityPolicyDetailsDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectQuery = @"select * from SecurityPolicyDetails";
            if (searchParameters != null && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<SecurityPolicyDetailsDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        {
                            if (searchParameter.Key.Equals(SecurityPolicyDetailsDTO.SearchByParameters.POLICY_DETAIL_ID) || searchParameter.Key.Equals(SecurityPolicyDetailsDTO.SearchByParameters.POLICY_ID))
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + searchParameter.Value);
                            }
                            else
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = '" + searchParameter.Value + "'");
                            }
                        }
                        count++;
                    }
                    else
                    {
                        log.Error("Ends-GetAllSecurityPolicyDetails(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception();
                    }
                }
                if (searchParameters.Count > 0)
                    selectQuery = selectQuery + query;
            }

            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, null, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                List<SecurityPolicyDetailsDTO> securityPolicyDetailsDTOList = new List<SecurityPolicyDetailsDTO>();
                foreach (DataRow securityPolicyDetailsDataRow in dataTable.Rows)
                {
                    SecurityPolicyDetailsDTO securityPolicyDetailsDataObject = GetSecurityPolicyDetailsDTO(securityPolicyDetailsDataRow);
                    securityPolicyDetailsDTOList.Add(securityPolicyDetailsDataObject);
                }
                log.LogMethodExit(securityPolicyDetailsDTOList);
                return securityPolicyDetailsDTOList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating SecurityPolicyDetails  Record.
        /// </summary>
        /// <param name="securityPolicyDetailsDTO">SecurityPolicyDetailsDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        /// Modified to access dataAccessHandler.GetSQLParameter() method by Mushahid Faizan on 09-Apr-2019
        private List<SqlParameter> GetSQLParameters(SecurityPolicyDetailsDTO securityPolicyDetailsDTO, string userId, int siteId)
        {
            log.LogMethodEntry(securityPolicyDetailsDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@id", securityPolicyDetailsDTO.PolicyDetailId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@policyId", securityPolicyDetailsDTO.PolicyId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@passwordChangeFrequency", securityPolicyDetailsDTO.PasswordChangeFrequency, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@passwordMinLength", securityPolicyDetailsDTO.PasswordMinLength, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@passwordMinAlphabets", securityPolicyDetailsDTO.PasswordMinAlphabets, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@passwordMinNumbers", securityPolicyDetailsDTO.PasswordMinNumbers, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@passwordMinSpecialChars", securityPolicyDetailsDTO.PasswordMinSpecialChars, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@rememberPasswordsCount", securityPolicyDetailsDTO.RememberPasswordsCount, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@invalidAttemptsBeforeLockout", securityPolicyDetailsDTO.InvalidAttemptsBeforeLockout, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lockoutDuration", securityPolicyDetailsDTO.LockoutDuration, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@userSessionTimeout", securityPolicyDetailsDTO.UserSessionTimeout, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@maxUserInactivityDays", securityPolicyDetailsDTO.MaxUserInactivityDays, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@maxDaysToLoginAfterUserCreation", securityPolicyDetailsDTO.MaxDaysToLoginAfterUserCreation, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", securityPolicyDetailsDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", securityPolicyDetailsDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the security policy details record to the database
        /// </summary>
        /// <param name="securityPolicyDetailsDTO">SecurityPolicyDetailsDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        /// Table name Mismatch: Modified by Mushahid Faizan on 09-Apr-2019
        public int InsertSecurityPolicyDetails(SecurityPolicyDetailsDTO securityPolicyDetailsDTO, string userId, int siteId)
        {
            log.LogMethodEntry(securityPolicyDetailsDTO, userId, siteId);
            int idOfRowInserted;
            string query = @"insert into SecurityPolicyDetails 
                                                        (                                                         
                                                        PolicyId,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastUpdatedDate,
                                                        site_id,
                                                        Guid,
                                                        SynchStatus,
                                                        PasswordChangeFrequency,
                                                        PasswordMinLength,
                                                        PasswordMinAlphabets,
                                                        PasswordMinNumbers,
                                                        PasswordMinSpecialChars,
                                                        RememberPasswordsCount,
                                                        InvalidAttemptsBeforeLockout,
                                                        LockoutDuration,
                                                        UserSessionTimeout,
                                                        MaxUserInactivityDays,
                                                        MaxDaysToLoginAfterUserCreation,
                                                        MasterEntityId,
                                                        IsActive
                                                        ) 
                                                values 
                                                        (                                                        
                                                        @policyId,
                                                        @createdBy,
                                                        GETDATE(),                                                        
                                                        @lastUpdatedBy,                                                        
                                                        GetDate(),
                                                        @siteId,
                                                        NewId(),
                                                        NULL,
                                                        @passwordChangeFrequency,
                                                        @passwordMinLength,
                                                        @passwordMinAlphabets,
                                                        @passwordMinNumbers,
                                                        @passwordMinSpecialChars,
                                                        @rememberPasswordsCount,
                                                        @invalidAttemptsBeforeLockout,
                                                        @lockoutDuration,
                                                        @userSessionTimeout,
                                                        @maxUserInactivityDays,
                                                        @maxDaysToLoginAfterUserCreation,
                                                        @masterEntityId,
                                                        @isActive                                                            
                                            )SELECT CAST(scope_identity() AS int)";
            try
            {
                idOfRowInserted = dataAccessHandler.executeInsertQuery(query, GetSQLParameters(securityPolicyDetailsDTO, userId, siteId).ToArray(), sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(idOfRowInserted);
            return idOfRowInserted;
        }

        /// <summary>
        /// Updates the security policy details record
        /// </summary>
        /// <param name="securityPolicyDTO">SecurityPolicyDetailsDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        /// Table name Mismatch: Modified by Mushahid Faizan on 09-Apr-2019
        public int UpdateSecurityPolicyDetails(SecurityPolicyDetailsDTO securityPolicyDetailsDTO, string userId, int siteId)
        {
            log.LogMethodEntry(securityPolicyDetailsDTO, userId, siteId);
            int rowsUpdated;
            string query = @"update SecurityPolicyDetails 
                                         set PolicyId=@policyId,                                             
                                             --site_id = @siteId,
                                             MasterEntityId = @masterEntityId,
                                             LastUpdatedBy = @lastUpdatedBy,
                                             LastUpdatedDate = GETDATE(),
                                             PasswordChangeFrequency = @passwordChangeFrequency,
                                             PasswordMinLength = @passwordMinLength,
                                             PasswordMinAlphabets = @passwordMinAlphabets,
                                             PasswordMinNumbers = @passwordMinNumbers,
                                             PasswordMinSpecialChars = @passwordMinSpecialChars,
                                             RememberPasswordsCount = @rememberPasswordsCount,
                                             InvalidAttemptsBeforeLockout = @invalidAttemptsBeforeLockout,
                                             LockoutDuration = @lockoutDuration,
                                             UserSessionTimeout = @userSessionTimeout,
                                             MaxUserInactivityDays = @maxUserInactivityDays,
                                             MaxDaysToLoginAfterUserCreation = @maxDaysToLoginAfterUserCreation,
                                             IsActive= @isActive
                                       where Id = @id";

            try
            {
                rowsUpdated = dataAccessHandler.executeUpdateQuery(query, GetSQLParameters(securityPolicyDetailsDTO, userId, siteId).ToArray(), sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(rowsUpdated);
            return rowsUpdated;
        }

        /// <summary>
        /// Based on the policyDetailsId, appropriate Security Policy details record will be deleted
        /// This is for hard deletion. In future, when we implement soft deletion this method may not be required
        /// </summary>
        /// <param name="policyDetailsId">primary key of SecurityPolicyDetails </param>
        /// <returns>return the int </returns>
        public int DeleteSecurityPolicyDetails(int policyDetailsId)
        {
            log.LogMethodEntry(policyDetailsId);
            try
            {
                string deleteQuery = @"delete from SecurityPolicyDetails where Id = @policyDetailsId";
                SqlParameter[] deleteParameters = new SqlParameter[1];
                deleteParameters[0] = new SqlParameter("@policyDetailsId", policyDetailsId);

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

