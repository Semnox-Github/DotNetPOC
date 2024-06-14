/********************************************************************************************
 * Project Name - RedemptionCurrencyRule Data Handler
 * Description  - Data handler of the RedemptionCurrencyRule class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.70.2       19-Aug-2019    Dakshakh raj        Created
 *2.70.2       10-Dec-2019   Jinto Thomas         Removed siteid from update query
 *2.110.0        21-oct-2019     Mushahid Faizan      inventory enhancement
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Redemption
{
    public class RedemptionCurrencyRuleDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"Select * from RedemptionCurrencyRule as rcr ";
        private List<SqlParameter> parameters = new List<SqlParameter>();

        /// <summary>
        /// Dictionary for searching Parameters for the RedemptionCurrencyRule object.
        /// </summary>
        private static readonly Dictionary<RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters, string> DBSearchParameters = new Dictionary<RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters, string>
            {
                {RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters.REDEMPTION_CURRENCY_RULE_ID, "rcr.RedemptionCurrencyRuleId"},
                {RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters.REDEMPTION_CURRENCY_RULE_NAME, "rcr.RedemptionCurrencyRuleName"},
                {RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters.SITE_ID, "rcr.site_id"},
                {RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters.IS_ACTIVE , "rcr.IsActive"},
                {RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters.MASTER_ENTITY_ID, "rcr.MasterEntityId"},
                {RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters.REDEMPTION_CURRENCY_RULE_ID_LIST,"rcr.RedemptionCurrencyRuleId"}

            };

        /// <summary>
        /// Default constructor of RedemptionCurrencyRuleDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public RedemptionCurrencyRuleDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodEntry();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating RedemptionCurrencyRuleDTO parameters Record.
        /// </summary>
        /// <param name="redemptionCurrencyRuleDTO">redemptionCurrencyRuleDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>  Returns the list of SQL parameter </returns>
        private List<SqlParameter> GetSQLParameters(RedemptionCurrencyRuleDTO redemptionCurrencyRuleDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(redemptionCurrencyRuleDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@redemptionCurrencyRuleId", redemptionCurrencyRuleDTO.RedemptionCurrencyRuleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@redemptionCurrencyRuleName", string.IsNullOrEmpty(redemptionCurrencyRuleDTO.RedemptionCurrencyRuleName) ? string.Empty : (object)redemptionCurrencyRuleDTO.RedemptionCurrencyRuleName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@description", string.IsNullOrEmpty(redemptionCurrencyRuleDTO.Description) ? string.Empty : (object)redemptionCurrencyRuleDTO.Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@percentage", redemptionCurrencyRuleDTO.Percentage == 0 ? DBNull.Value : (object)redemptionCurrencyRuleDTO.Percentage));
            parameters.Add(dataAccessHandler.GetSQLParameter("@amount", redemptionCurrencyRuleDTO.Amount == 0 ? DBNull.Value : (object)redemptionCurrencyRuleDTO.Amount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@priority", redemptionCurrencyRuleDTO.Priority));
            parameters.Add(dataAccessHandler.GetSQLParameter("@cumulative", redemptionCurrencyRuleDTO.Cumulative));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", redemptionCurrencyRuleDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", redemptionCurrencyRuleDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the RedemptionCurrencyRule record to the database
        /// </summary>
        /// <param name="redemptionCurrencyRuleDTO">redemptionCurrencyRuleDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>redemptionCurrencyRuleDetailDTO</returns>
        public RedemptionCurrencyRuleDTO InsertRedemptionCurrencyRule(RedemptionCurrencyRuleDTO redemptionCurrencyRuleDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(redemptionCurrencyRuleDTO, loginId, siteId);
            string query = @"INSERT INTO RedemptionCurrencyRule
                                        ( 
                                            RedemptionCurrencyRuleName,
                                            Description,
                                            Percentage,
                                            Amount,
                                            Priority,
                                            Cumulative,
                                            IsActive,
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdatedBy,
                                            LastUpdateDate,
                                            site_id,
                                            Guid,
                                            MasterEntityId
                                        ) 
                                VALUES 
                                        (
                                            @redemptionCurrencyRuleName,
                                            @description,
                                            @percentage,
                                            @amount,
                                            @priority,
                                            @cumulative,            
                                            @isActive,
                                            @createdBy,
                                            GETDATE(),
                                            @lastUpdatedBy,
                                            GETDATE(),
                                            @site_id,
                                            NEWID(),
                                            @masterEntityId
                                        )SELECT * FROM RedemptionCurrencyRule WHERE RedemptionCurrencyRuleId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(redemptionCurrencyRuleDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshRedemptionCurrencyRuleDTO(redemptionCurrencyRuleDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting redemptionCurrencyRuleDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(redemptionCurrencyRuleDTO);
            return redemptionCurrencyRuleDTO;
        }

        /// <summary>
        /// Updates the RedemptionCurrencyRule record
        /// </summary>
        /// <param name="redemptionCurrencyRuleDTO">RedemptionCurrencyRuleDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>RedemptionCurrencyRuleDTO</returns>
        public RedemptionCurrencyRuleDTO UpdateRedemptionCurrencyRule(RedemptionCurrencyRuleDTO redemptionCurrencyRuleDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(redemptionCurrencyRuleDTO, loginId, siteId);
            string query = @"UPDATE RedemptionCurrencyRule 
                             SET RedemptionCurrencyRuleName=@redemptionCurrencyRuleName,
                                 Description=@description,
                                 Percentage=@percentage,
                                 Amount=@amount, 
                                 Priority=@priority, 
                                 Cumulative=@cumulative, 
                                 IsActive=@isActive, 
                                 LastUpdatedBy=@lastUpdatedBy,
                                 LastUpdateDate= GETDATE(),
                                 --site_id=@site_id,
                                 MasterEntityId=@masterEntityId
                             WHERE RedemptionCurrencyRuleId = @redemptionCurrencyRuleId
                             SELECT * FROM RedemptionCurrencyRule WHERE RedemptionCurrencyRuleId = @redemptionCurrencyRuleId ";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(redemptionCurrencyRuleDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshRedemptionCurrencyRuleDTO(redemptionCurrencyRuleDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating redemptionCurrencyRuleDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(redemptionCurrencyRuleDTO);
            return redemptionCurrencyRuleDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="redemptionCurrencyRuleDTO">redemptionCurrencyRuleDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshRedemptionCurrencyRuleDTO(RedemptionCurrencyRuleDTO redemptionCurrencyRuleDTO, DataTable dt)
        {
            log.LogMethodEntry(redemptionCurrencyRuleDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                redemptionCurrencyRuleDTO.RedemptionCurrencyRuleId = Convert.ToInt32(dt.Rows[0]["RedemptionCurrencyRuleId"]);
                redemptionCurrencyRuleDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                redemptionCurrencyRuleDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                redemptionCurrencyRuleDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                redemptionCurrencyRuleDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                redemptionCurrencyRuleDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                redemptionCurrencyRuleDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to RedemptionCurrencyRuleDTO class type
        /// </summary>
        /// <param name="redemptionCurrencyRuleDataRow">redemptionCurrencyRule DataRow</param>
        /// <returns>Returns redemptionCurrencyRule</returns>
        private RedemptionCurrencyRuleDTO GetRedemptionCurrencyRuleDTO(DataRow redemptionCurrencyRuleDataRow)
        {
            log.LogMethodEntry(redemptionCurrencyRuleDataRow);
            RedemptionCurrencyRuleDTO redemptionCurrencyRuleDataObject = new RedemptionCurrencyRuleDTO(Convert.ToInt32(redemptionCurrencyRuleDataRow["RedemptionCurrencyRuleId"]),
                                            redemptionCurrencyRuleDataRow["RedemptionCurrencyRuleName"] == DBNull.Value ? string.Empty : redemptionCurrencyRuleDataRow["RedemptionCurrencyRuleName"].ToString(),
                                            redemptionCurrencyRuleDataRow["Description"] == DBNull.Value ? string.Empty : redemptionCurrencyRuleDataRow["Description"].ToString(),
                                            redemptionCurrencyRuleDataRow["Percentage"] == DBNull.Value ? 0 : Convert.ToDecimal(redemptionCurrencyRuleDataRow["Percentage"]),
                                            redemptionCurrencyRuleDataRow["Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(redemptionCurrencyRuleDataRow["Amount"]),
                                            Convert.ToInt32(redemptionCurrencyRuleDataRow["Priority"]),
                                            redemptionCurrencyRuleDataRow["Cumulative"] == DBNull.Value ? false : Convert.ToBoolean(redemptionCurrencyRuleDataRow["Cumulative"]),
                                            redemptionCurrencyRuleDataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(redemptionCurrencyRuleDataRow["IsActive"]),
                                            redemptionCurrencyRuleDataRow["CreatedBy"] == DBNull.Value ? string.Empty : redemptionCurrencyRuleDataRow["CreatedBy"].ToString(),
                                            redemptionCurrencyRuleDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(redemptionCurrencyRuleDataRow["CreationDate"]),
                                            redemptionCurrencyRuleDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : redemptionCurrencyRuleDataRow["LastUpdatedBy"].ToString(),
                                            redemptionCurrencyRuleDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(redemptionCurrencyRuleDataRow["LastUpdateDate"]),
                                            redemptionCurrencyRuleDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(redemptionCurrencyRuleDataRow["site_id"]),
                                            redemptionCurrencyRuleDataRow["Guid"] == DBNull.Value ? string.Empty : redemptionCurrencyRuleDataRow["Guid"].ToString(),
                                            redemptionCurrencyRuleDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(redemptionCurrencyRuleDataRow["SynchStatus"]),
                                            redemptionCurrencyRuleDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(redemptionCurrencyRuleDataRow["MasterEntityId"])
                                            );
            log.LogMethodExit(redemptionCurrencyRuleDataObject);
            return redemptionCurrencyRuleDataObject;
        }

        /// <summary>
        /// Gets the redemption Currency Rule Detail of passed RedemptionCurrencyRuleId
        /// </summary>
        /// <param name="redemptionCurrencyRuleId">integer type parameter</param>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        /// <returns>Returns RedemptionCurrencyRuleDTO</returns>
        public RedemptionCurrencyRuleDTO GetRedemptionCurrencyRule(int redemptionCurrencyRuleId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(redemptionCurrencyRuleId, sqlTransaction);
            RedemptionCurrencyRuleDTO result = null;
            string selectRedemptionCurrencyRuleQuery = SELECT_QUERY + @" WHERE RedemptionCurrencyRuleId = @redemptionCurrencyRuleId";
            SqlParameter parameter = new SqlParameter("@redemptionCurrencyRuleId", redemptionCurrencyRuleId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectRedemptionCurrencyRuleQuery, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetRedemptionCurrencyRuleDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the no of RedemptionCurrencyRule matching the search parameters
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>no of accounts matching the criteria</returns>
        public int GetCurrencyRulesCount(List<KeyValuePair<RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters, string>> searchParameters)
        {
            log.LogMethodEntry();
            int currencyRuleDTOCount = 0;
            string selectQuery = SELECT_QUERY;
            selectQuery += GetFilterQuery(searchParameters);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                currencyRuleDTOCount = Convert.ToInt32(dataTable.Rows.Count);
            }
            log.LogMethodExit(currencyRuleDTOCount);
            return currencyRuleDTOCount;
        }

        /// <summary>
        /// Builds the List of query based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of query</returns>
        public string GetFilterQuery(List<KeyValuePair<RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            StringBuilder query = new StringBuilder(" ");

            if (searchParameters != null && (searchParameters.Count > 0))
            {
                query.Append(" where ");
                string joiner;
                foreach (KeyValuePair<RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters, string> searchParameter in searchParameters)
                {
                    joiner = count == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters.REDEMPTION_CURRENCY_RULE_ID
                            || searchParameter.Key == RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters.REDEMPTION_CURRENCY_RULE_NAME)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'0')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else if (searchParameter.Key == RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters.REDEMPTION_CURRENCY_RULE_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
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
        /// Gets the RedemptionCurrencyRuleDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        /// <returns>Returns the list of RedemptionCurrencyRuleDTO matching the search criteria</returns>
        public List<RedemptionCurrencyRuleDTO> GetRedemptionCurrencyRuleDTOList(List<KeyValuePair<RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters, string>> searchParameters, int currentPage, int pageSize, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<RedemptionCurrencyRuleDTO> redemptionCurrencyRuleDTOList = new List<RedemptionCurrencyRuleDTO>();
            parameters.Clear();
            string selectQuery = SELECT_QUERY;
            selectQuery += GetFilterQuery(searchParameters);
            if (currentPage > 0 && pageSize > 0)
            {
                selectQuery += " ORDER BY rcr.RedemptionCurrencyRuleId OFFSET " + (currentPage * pageSize).ToString() + " ROWS";
                selectQuery += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                redemptionCurrencyRuleDTOList = new List<RedemptionCurrencyRuleDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    RedemptionCurrencyRuleDTO redemptionCurrencyRuleDTO = GetRedemptionCurrencyRuleDTO(dataRow);
                    redemptionCurrencyRuleDTOList.Add(redemptionCurrencyRuleDTO);
                }
            }
            log.LogMethodExit(redemptionCurrencyRuleDTOList);
            return redemptionCurrencyRuleDTOList;
        }
        /// <summary>
        /// Gets the RedemptionCurrencyRuleDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        /// <returns>Returns the list of RedemptionCurrencyRuleDTO matching the search criteria</returns>
        public List<RedemptionCurrencyRuleDTO> GetRedemptionCurrencyRuleDTOList(List<KeyValuePair<RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<RedemptionCurrencyRuleDTO> redemptionCurrencyRuleDTOList = new List<RedemptionCurrencyRuleDTO>();
            parameters.Clear();
            string selectQuery = SELECT_QUERY;
            selectQuery += GetFilterQuery(searchParameters);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                redemptionCurrencyRuleDTOList = new List<RedemptionCurrencyRuleDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    RedemptionCurrencyRuleDTO redemptionCurrencyRuleDTO = GetRedemptionCurrencyRuleDTO(dataRow);
                    redemptionCurrencyRuleDTOList.Add(redemptionCurrencyRuleDTO);
                }
            }
            log.LogMethodExit(redemptionCurrencyRuleDTOList);
            return redemptionCurrencyRuleDTOList;
        }

        internal DateTime? GetRedemptionCurrencyRuleModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"SELECT MAX(LastUpdatedDate) LastUpdatedDate 
                            FROM (
                            select max(last_updated_date) LastUpdatedDate from games WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastUpdateDate) LastUpdatedDate from GameProfileAttributes WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastUpdatedDate) LastUpdatedDate from GameProfileAttributeValues WHERE (site_id = @siteId or @siteId = -1) and machine_id is null
                            ) a";
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
    }
}
