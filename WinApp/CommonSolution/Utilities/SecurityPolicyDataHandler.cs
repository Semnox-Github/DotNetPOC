/********************************************************************************************
 * Project Name - Utilities
 * Description  - Get and Insert or update methods for Security policy details.
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70        24-Mar-2019   Jagan Mohana          Created 
 *            09-Apr-2019   Mushahid Faizan       Modified- GetSQLParameters() & GetSecurityPolicyDTO(), Insert/Update method.
 *                                                Added DeleteSecurityPolicy() method.
 *            29-Jul-2019   Mushahid Faizan       Added IsActive & SiteId Search Parameters
 *2.70.2        11-Dec-2019   Jinto Thomas          Removed siteid from update query            
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Semnox.Core.Utilities
{
    public class SecurityPolicyDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;
        private static readonly Dictionary<SecurityPolicyDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<SecurityPolicyDTO.SearchByParameters, string>
        {
            { SecurityPolicyDTO.SearchByParameters.POLICY_ID,"PolicyId"},
            { SecurityPolicyDTO.SearchByParameters.POLICY_NAME, "PolicyName"},
            { SecurityPolicyDTO.SearchByParameters.SITEID, "site_id"}
        };

        /// <summary>
        /// Default constructor of SecurityPolicyDataHandler class
        /// </summary>
        public SecurityPolicyDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            this.sqlTransaction = sqlTransaction;
            this.dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        ///  Converts the Data row object to SecurityPolicyDTO class type
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        /// Modified site_id & LastUpdatedDate column by Mushahid Faizan on 09-Apr-2019
        private SecurityPolicyDTO GetSecurityPolicyDTO(DataRow dataRow)
        {
            log.LogMethodEntry();
            SecurityPolicyDTO securityPolicyDTO = new SecurityPolicyDTO(Convert.ToInt32(dataRow["PolicyId"]),
                                            dataRow["PolicyName"] == DBNull.Value ? "" : dataRow["PolicyName"].ToString(),
                                            dataRow["CreatedBy"] == DBNull.Value ? "" : Convert.ToString(dataRow["CreatedBy"]),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? "" : Convert.ToString(dataRow["LastUpdatedBy"]),
                                            dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["Guid"] == DBNull.Value ? "" : dataRow["Guid"].ToString(),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"])
                                            );
            log.LogMethodExit(securityPolicyDTO);
            return securityPolicyDTO;
        }

        /// <summary>
        /// Gets the SecurityPolicyDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of SecurityPolicyDTO matching the search criteria</returns>
        public List<SecurityPolicyDTO> GetAllSecurityPolicy(List<KeyValuePair<SecurityPolicyDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectQuery = @"select * from SecurityPolicy";
            if (searchParameters != null && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<SecurityPolicyDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key.Equals(SecurityPolicyDTO.SearchByParameters.POLICY_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + searchParameter.Value);
                        }
                        else if (searchParameter.Key == SecurityPolicyDTO.SearchByParameters.SITEID)
                        {
                            query.Append(joiner + " (" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                        }
                        else
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = '" + searchParameter.Value + "'");
                        }
                        count++;
                    }
                    else
                    {
                        log.Error("Ends-GetAllSecurityPolicy(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception();
                    }
                }
                if (searchParameters.Count > 0)
                    selectQuery = selectQuery + query;
            }

            DataTable companyData = dataAccessHandler.executeSelectQuery(selectQuery, null, sqlTransaction);
            if (companyData.Rows.Count > 0)
            {
                List<SecurityPolicyDTO> securityPolicyDTOList = new List<SecurityPolicyDTO>();
                foreach (DataRow securityPolicyDataRow in companyData.Rows)
                {
                    SecurityPolicyDTO securityPolicyDataObject = GetSecurityPolicyDTO(securityPolicyDataRow);
                    securityPolicyDTOList.Add(securityPolicyDataObject);
                }
                log.LogMethodExit(securityPolicyDTOList);
                return securityPolicyDTOList;
            }
            else
            {
                log.LogMethodExit(null);
                return null;
            }
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating task types Record.
        /// </summary>
        /// <param name="securityPolicyDTO">SecurityPolicyDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        /// Modified to access dataAccessHandler.GetSQLParameter() method by Mushahid Faizan on 09-Apr-2019
        private List<SqlParameter> GetSQLParameters(SecurityPolicyDTO securityPolicyDTO, string userId, int siteId)
        {
            log.LogMethodEntry(securityPolicyDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@policyId", securityPolicyDTO.PolicyId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@policyName", securityPolicyDTO.PolicyName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", securityPolicyDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", securityPolicyDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }

        internal DateTime? GetSecurityPolicyModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"SELECT MAX(LastUpdatedDate) LastUpdatedDate
                            FROM (
                            select max(LastUpdatedDate) LastUpdatedDate from SecurityPolicy WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastUpdatedDate) LastUpdatedDate from SecurityPolicyDetails WHERE (site_id = @siteId or @siteId = -1)) a";
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
        /// Inserts the security policy record to the database
        /// </summary>
        /// <param name="securityPolicyDTO">SecurityPolicyDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertSecurityPolicy(SecurityPolicyDTO securityPolicyDTO, string userId, int siteId) //Added comma after CreationDate by Mushahid Faizan
        {
            log.LogMethodEntry(securityPolicyDTO, userId, siteId);
            int idOfRowInserted;
            string query = @"insert into SecurityPolicy 
                                                        (
                                                        PolicyName,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastUpdatedDate,
                                                        site_id,
                                                        Guid,
                                                        MasterEntityId,
                                                        IsActive
                                                        ) 
                                                values 
                                                        (                                                        
                                                        @policyName,
                                                        @createdBy,
                                                        GETDATE(),                                                        
                                                        @lastUpdatedBy,                                                        
                                                        GetDate(),
                                                        @siteId,
                                                        NewId(),
                                                        @masterEntityId,
                                                        @isActive   
                                            )SELECT CAST(scope_identity() AS int)";
            try
            {
                idOfRowInserted = dataAccessHandler.executeInsertQuery(query, GetSQLParameters(securityPolicyDTO, userId, siteId).ToArray(), sqlTransaction);
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
        /// Updates the security policy record
        /// </summary>
        /// <param name="securityPolicyDTO">SecurityPolicyDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        /// Table name Mismatch: Modified by Mushahid Faizan on 09-Apr-2019
        public int UpdateSecurityPolicy(SecurityPolicyDTO securityPolicyDTO, string userId, int siteId)
        {
            log.LogMethodEntry(securityPolicyDTO, userId, siteId);
            int rowsUpdated;
            string query = @"update SecurityPolicy 
                                         set PolicyName=@policyName,                                             
                                             -- site_id = @siteId,
                                             MasterEntityId = @masterEntityId,
                                             LastUpdatedBy = @lastUpdatedBy,
                                             LastUpdatedDate = GETDATE(),
                                             IsActive= @isActive
                                       where PolicyId = @policyId";

            try
            {
                rowsUpdated = dataAccessHandler.executeUpdateQuery(query, GetSQLParameters(securityPolicyDTO, userId, siteId).ToArray(), sqlTransaction);
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
        /// Gets the Security data of passed securityPolicyId
        /// </summary>
        /// <param name="securityPolicyId">securityPolicyId</param>
        /// <returns>Returns SecurityPolicyDTO</returns>
        public SecurityPolicyDTO GetSecurityPolicyDTO(int securityPolicyId)
        {
            log.LogMethodEntry(securityPolicyId);
            try
            {
                string selectQuery = @"select *
                                         from SecurityPolicy
                                        where PolicyId = @policyId";
                SqlParameter[] selectParameters = new SqlParameter[1];
                selectParameters[0] = new SqlParameter("@policyId", securityPolicyId);
                DataTable securityPolicyData = dataAccessHandler.executeSelectQuery(selectQuery, selectParameters, sqlTransaction);
                if (securityPolicyData.Rows.Count > 0)
                {
                    DataRow securityPolicyDataRow = securityPolicyData.Rows[0];
                    SecurityPolicyDTO securityPolicyDataObject = GetSecurityPolicyDTO(securityPolicyDataRow);
                    log.LogMethodExit(securityPolicyDataObject);
                    return securityPolicyDataObject;
                }
                else
                {
                    log.LogMethodExit();
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "throwing exception");
                throw;
            }
        }

        /// <summary>
        /// Based on the policyId, appropriate SecurityPolicy record will be deleted
        /// This is for hard deletion. In future, when we implement soft deletion this method may not be required
        /// </summary>
        /// <param name="policyId">primary key of SecurityPolicy </param>
        /// <returns>return the int </returns>
        public int DeleteSecurityPolicy(int policyId)
        {
            log.LogMethodEntry(policyId);
            try
            {
                string deleteQuery = @"delete from SecurityPolicy where PolicyId = @policyId";
                SqlParameter[] deleteParameters = new SqlParameter[1];
                deleteParameters[0] = new SqlParameter("@policyId", policyId);

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