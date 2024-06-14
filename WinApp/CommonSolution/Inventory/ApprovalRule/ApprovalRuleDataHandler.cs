/********************************************************************************************
 * Project Name - Approval Rule Data Handler
 * Description  - Data handler of the approval rule class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00          18-OCT-2017   Raghuveera          Created 
 *2.70.2        13-Aug-2019   Deeksha             modifications as per 3 tier standards
 *2.70.2        09-Dec-2019   Jinto Thomas        Removed siteid from update query 
 *2.110.0       14-Oct-2020   Mushahid Faizan     Added methods for Pagination and modified search filters method .
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// Approval Rule Data Handler - Handles insert, update and select of approval rule objects
    /// </summary>
    public class ApprovalRuleDataHandler
    {
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM ApprovalRule AS ar ";
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<ApprovalRuleDTO.SearchByApprovalRuleParameters, string> DBSearchParameters = new Dictionary<ApprovalRuleDTO.SearchByApprovalRuleParameters, string>
            {
                {ApprovalRuleDTO.SearchByApprovalRuleParameters.APPROVAL_RULE_ID, "ar.ApprovalRuleID"},
                {ApprovalRuleDTO.SearchByApprovalRuleParameters.DOCUMENT_TYPE_ID, "ar.DocumentTypeID"},
                {ApprovalRuleDTO.SearchByApprovalRuleParameters.ROLE_ID, "ar.role_id"},
                {ApprovalRuleDTO.SearchByApprovalRuleParameters.NUMBER_OF_APPROVAL_LEVELS, "ar.NumberOfApprovalLevels"},
                {ApprovalRuleDTO.SearchByApprovalRuleParameters.ACTIVE_FLAG, "ar.IsActive"},
                {ApprovalRuleDTO.SearchByApprovalRuleParameters.MASTER_ENTITY_ID,"ar.MasterEntityId"},
                {ApprovalRuleDTO.SearchByApprovalRuleParameters.SITE_ID, "ar.site_id"}
            };
        private DataAccessHandler dataAccessHandler;
        private List<SqlParameter> parameters = new List<SqlParameter>();

        /// <summary>
        /// Default constructor of ApprovalRuleDataHandler class
        /// </summary>
        public ApprovalRuleDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating InventoryReceipt Record.
        /// </summary>
        /// <param name="ApprovalRuleDTO">ApprovalRuleDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(ApprovalRuleDTO approvalRuleDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(approvalRuleDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@approvalRuleID", approvalRuleDTO.ApprovalRuleID, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@documentTypeID", approvalRuleDTO.DocumentTypeID, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@roleId", approvalRuleDTO.RoleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@numberOfApprovalLevels", approvalRuleDTO.NumberOfApprovalLevels, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", approvalRuleDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", approvalRuleDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteid", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the approval rule record to the database
        /// </summary>
        /// <param name="approvalRule">ApprovalRuleDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public ApprovalRuleDTO InsertApprovalRule(ApprovalRuleDTO approvalRule, string loginId, int siteId)
        {
            //loginId = "semnx";
            log.LogMethodEntry(approvalRule, loginId, siteId);
            string insertApprovalRuleQuery = @"insert into ApprovalRule 
                                                        (
                                                        DocumentTypeID,
                                                        role_id,
                                                        NumberOfApprovalLevels,
                                                        MasterEntityId,
                                                        IsActive,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastUpdatedDate,
                                                        Guid,
                                                        site_id
                                                        ) 
                                                values 
                                                        (
                                                         @documentTypeID,
                                                         @roleId,
                                                         @numberOfApprovalLevels,
                                                         @masterEntityId,
                                                         @isActive,
                                                         @createdBy,
                                                         Getdate(),                                                         
                                                         @createdBy,
                                                         Getdate(),                                                         
                                                         NEWID(),
                                                         @siteid
                                                        ) SELECT * FROM ApprovalRule WHERE ApprovalRuleID = scope_identity()";


            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertApprovalRuleQuery, GetSQLParameters(approvalRule, loginId, siteId).ToArray(), sqlTransaction);
                RefreshApprovalRuleDTO(approvalRule, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting approvalRule", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(approvalRule);
            return approvalRule;
        }

        /// <summary>
        /// Updates the approval rule record
        /// </summary>
        /// <param name="approvalRule">ApprovalRuleDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public ApprovalRuleDTO UpdateApprovalRule(ApprovalRuleDTO approvalRule, string loginId, int siteId)
        {
            //loginId = "semnx";
            log.LogMethodEntry(approvalRule, loginId, siteId); ;
            string updateApprovalRuleQuery = @"update ApprovalRule 
                                         set DocumentTypeID = @documentTypeID,
                                             role_id = @roleId,
                                             NumberOfApprovalLevels = @numberOfApprovalLevels,
                                             MasterEntityId=@masterEntityId,
                                             IsActive = @isActive, 
                                             LastUpdatedBy = @lastUpdatedBy, 
                                             LastupdatedDate = Getdate()
                                             --site_id=@siteid                                         
                                       where ApprovalRuleID = @approvalRuleID
                            SELECT * FROM ApprovalRule WHERE ApprovalRuleID = @approvalRuleID";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateApprovalRuleQuery, GetSQLParameters(approvalRule, loginId, siteId).ToArray(), sqlTransaction);
                RefreshApprovalRuleDTO(approvalRule, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating approvalRule", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(approvalRule);
            return approvalRule;
        }

        /// <summary>
        /// Delete the record from the ApprovalRule database based on ApprovalRuleID
        /// </summary>
        /// <param name="ApprovalRuleID">ApprovalRuleID</param>
        /// <returns>return the int </returns>
        internal int Delete(int approvalRuleID)
        {
            log.LogMethodEntry(approvalRuleID);
            string query = @"DELETE  
                             FROM ApprovalRule
                             WHERE ApprovalRule.ApprovalRuleID = @approvalRuleID";
            SqlParameter parameter = new SqlParameter("@approvalRuleID", approvalRuleID);
            int id = dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit(id);
            return id;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="ApprovalRuleDTO">ApprovalRuleDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshApprovalRuleDTO(ApprovalRuleDTO approvalRule, DataTable dt)
        {
            log.LogMethodEntry(approvalRule, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                approvalRule.ApprovalRuleID = Convert.ToInt32(dt.Rows[0]["ApprovalRuleID"]);
                approvalRule.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                approvalRule.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                approvalRule.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                approvalRule.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                approvalRule.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                approvalRule.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to ApprovalRuleDTO class type
        /// </summary>
        /// <param name="approvalRuleDataRow">ApprovalRule DataRow</param>
        /// <returns>Returns ApprovalRule</returns>
        private ApprovalRuleDTO GetApprovalRuleDTO(DataRow approvalRuleDataRow)
        {
            log.LogMethodEntry(approvalRuleDataRow);
            ApprovalRuleDTO approvalRuleDataObject = new ApprovalRuleDTO(Convert.ToInt32(approvalRuleDataRow["ApprovalRuleID"]),
                            approvalRuleDataRow["DocumentTypeID"] == DBNull.Value ? -1 : Convert.ToInt32(approvalRuleDataRow["DocumentTypeID"]),
                            approvalRuleDataRow["role_id"] == DBNull.Value ? -1 : Convert.ToInt32(approvalRuleDataRow["role_id"]),
                            approvalRuleDataRow["NumberOfApprovalLevels"] == DBNull.Value ? 0 : Convert.ToInt32(approvalRuleDataRow["NumberOfApprovalLevels"]),
                            approvalRuleDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(approvalRuleDataRow["MasterEntityId"]),
                            approvalRuleDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(approvalRuleDataRow["IsActive"]),
                            approvalRuleDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(approvalRuleDataRow["CreatedBy"]),
                            approvalRuleDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(approvalRuleDataRow["CreationDate"]),
                            approvalRuleDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(approvalRuleDataRow["LastUpdatedBy"]),
                            approvalRuleDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(approvalRuleDataRow["LastupdatedDate"]),
                            approvalRuleDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(approvalRuleDataRow["Guid"]),
                            approvalRuleDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(approvalRuleDataRow["site_id"]),
                            approvalRuleDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(approvalRuleDataRow["SynchStatus"])
                            );
            log.LogMethodExit(approvalRuleDataObject);
            return approvalRuleDataObject;
        }

        /// <summary>
        /// Gets the approval rule data of passed approval rule Id
        /// </summary>
        /// <param name="approvalRuleId">integer type parameter</param>
        /// <returns>Returns ApprovalRuleDTO</returns>
        public ApprovalRuleDTO GetApprovalRule(int approvalRuleId)
        {
            log.LogMethodEntry(approvalRuleId);
            ApprovalRuleDTO result = null;
            string query = SELECT_QUERY + @" WHERE ar.ApprovalRuleID= @approvalRuleID";
            SqlParameter parameter = new SqlParameter("@approvalRuleID", approvalRuleId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetApprovalRuleDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the no of ApprovalRuleDTO matching the search parameters
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>no of accounts matching the criteria</returns>
        public int GetApprovalRuleCount(List<KeyValuePair<ApprovalRuleDTO.SearchByApprovalRuleParameters, string>> searchParameters)
        {
            log.LogMethodEntry();
            int count = 0;
            string selectQuery = SELECT_QUERY;
            selectQuery += GetFilterQuery(searchParameters);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                count = Convert.ToInt32(dataTable.Rows.Count);
            }
            log.LogMethodExit(count);
            return count;
        }

        /// <summary>
        /// Returns the sql query based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of query</returns>
        public string GetFilterQuery(List<KeyValuePair<ApprovalRuleDTO.SearchByApprovalRuleParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            StringBuilder query = new StringBuilder(" ");

            if (searchParameters != null && (searchParameters.Count > 0))
            {
                query.Append(" where ");
                string joiner;
                foreach (KeyValuePair<ApprovalRuleDTO.SearchByApprovalRuleParameters, string> searchParameter in searchParameters)
                {
                    joiner = count == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == ApprovalRuleDTO.SearchByApprovalRuleParameters.APPROVAL_RULE_ID
                            || searchParameter.Key == ApprovalRuleDTO.SearchByApprovalRuleParameters.NUMBER_OF_APPROVAL_LEVELS
                            || searchParameter.Key == ApprovalRuleDTO.SearchByApprovalRuleParameters.DOCUMENT_TYPE_ID
                            || searchParameter.Key == ApprovalRuleDTO.SearchByApprovalRuleParameters.ROLE_ID
                            || searchParameter.Key == ApprovalRuleDTO.SearchByApprovalRuleParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ApprovalRuleDTO.SearchByApprovalRuleParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ApprovalRuleDTO.SearchByApprovalRuleParameters.ACTIVE_FLAG)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
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
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                }
            }
            log.LogMethodExit(query);
            return query.ToString();
        }

        /// <summary>
        /// Gets the ApprovalRuleDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ApprovalRuleDTO matching the search criteria</returns>
        public List<ApprovalRuleDTO> GetApprovalRuleList(List<KeyValuePair<ApprovalRuleDTO.SearchByApprovalRuleParameters, string>> searchParameters, int currentPage, int pageSize)
        {
            log.LogMethodEntry(searchParameters);
            List<ApprovalRuleDTO> approvalRuleDTOList = new List<ApprovalRuleDTO>();
            parameters.Clear();
            string selectQuery = SELECT_QUERY;
            selectQuery += GetFilterQuery(searchParameters);
            if (currentPage > 0 && pageSize > 0)
            {
                selectQuery += " ORDER BY ar.ApprovalRuleID OFFSET " + (currentPage * pageSize).ToString() + " ROWS";
                selectQuery += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                approvalRuleDTOList = new List<ApprovalRuleDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ApprovalRuleDTO approvalRuleDTO = GetApprovalRuleDTO(dataRow);
                    approvalRuleDTOList.Add(approvalRuleDTO);
                }
            }
            log.LogMethodExit(approvalRuleDTOList);
            return approvalRuleDTOList;
        }


        /// <summary>
        /// Gets the ApprovalRuleDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ApprovalRuleDTO matching the search criteria</returns>
        public List<ApprovalRuleDTO> GetApprovalRuleList(List<KeyValuePair<ApprovalRuleDTO.SearchByApprovalRuleParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<ApprovalRuleDTO> approvalRuleDTOList = new List<ApprovalRuleDTO>();
            parameters.Clear();
            string selectQuery = SELECT_QUERY;
            selectQuery += GetFilterQuery(searchParameters);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                approvalRuleDTOList = new List<ApprovalRuleDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ApprovalRuleDTO approvalRuleDTO = GetApprovalRuleDTO(dataRow);
                    approvalRuleDTOList.Add(approvalRuleDTO);
                }
            }
            log.LogMethodExit(approvalRuleDTOList);
            return approvalRuleDTOList;
        }
    }
}
