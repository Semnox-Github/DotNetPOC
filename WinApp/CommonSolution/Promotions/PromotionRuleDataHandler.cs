/********************************************************************************************
 * Project Name - Promotions
 * Description  - Data Handler File for PromotionRule
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks 
 *********************************************************************************************
 *2.70        10-June-2019   Divya A                 Created 
 *2.80        24-Apr-2020    Girish Kundar             Modified : Removed the search by PromotionIdList method .
 ********************************************************************************************/
using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Semnox.Core.Utilities;
using System.Linq;

namespace Semnox.Parafait.Promotions
{
    /// <summary>
    /// PromotionRule Data Handler - Handles insert, update and select of PromotionRule objects
    /// </summary>
    public class PromotionRuleDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM PromotionRule as pr ";

        /// <summary>
        /// Dictionary for searching Parameters for the PromotionRule object.
        /// </summary>
        private static readonly Dictionary<PromotionRuleDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<PromotionRuleDTO.SearchByParameters, string>
        {
            { PromotionRuleDTO.SearchByParameters.PROMOTION_RULE_ID,"pr.Id"},
            { PromotionRuleDTO.SearchByParameters.PROMOTION_ID,"pr.Promotion_Id"},
            { PromotionRuleDTO.SearchByParameters.PROMOTION_ID_LIST,"pr.Promotion_Id"},
            { PromotionRuleDTO.SearchByParameters.CARD_TYPE_ID,"pr.CardTypeId"},
            { PromotionRuleDTO.SearchByParameters.CAMPAIGN_ID,"pr.CampaignId"},
            { PromotionRuleDTO.SearchByParameters.MEMBERSHIP_ID,"pr.MembershipID"},
            { PromotionRuleDTO.SearchByParameters.SITE_ID,"pr.site_id"},
            { PromotionRuleDTO.SearchByParameters.IS_ACTIVE,"pr.IsActive"},
            { PromotionRuleDTO.SearchByParameters.MASTER_ENTITY_ID,"pr.MasterEntityId"}
        };

        /// <summary>
        /// Parameterized Constructor for PromotionRuleDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        public PromotionRuleDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating PromotionRule Record.
        /// </summary>
        /// <param name="promotionRuleDTO">promotionRuleDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(PromotionRuleDTO promotionRuleDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(promotionRuleDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", promotionRuleDTO.Id));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Promotion_Id", promotionRuleDTO.PromotionId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CardTypeId", promotionRuleDTO.CardTypeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CampaignId", promotionRuleDTO.CampaignId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MembershipID", promotionRuleDTO.MembershipId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", promotionRuleDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", promotionRuleDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Converts the Data row object to PromotionRuleDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the object of PromotionRuleDTO</returns>
        private PromotionRuleDTO GetPromotionRuleDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            PromotionRuleDTO promotionRuleDTO = new PromotionRuleDTO(dataRow["Id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Id"]),
                                                dataRow["Promotion_Id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Promotion_Id"]),
                                                dataRow["CardTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CardTypeId"]),
                                                dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                dataRow["CampaignId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CampaignId"]),
                                                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                dataRow["MembershipID"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MembershipID"]),
                                                dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                                dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]));
            log.LogMethodExit(promotionRuleDTO);
            return promotionRuleDTO;
        }

        /// <summary>
        /// Gets the PromotionRule data of passed PromotionRule ID
        /// </summary>
        /// <param name="prId">prId is passed as parameter</param>
        /// <returns>Returns PromotionRuleDTO</returns>
        public PromotionRuleDTO GetPromotionRuleDTO(int prId)
        {
            log.LogMethodEntry(prId);
            PromotionRuleDTO result = null;
            string query = SELECT_QUERY + @" WHERE prid.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", prId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetPromotionRuleDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        ///  Deletes the PromotionRule record
        /// </summary>
        /// <param name="promotionRuleDTO">PromotionRuleDTO is passed as parameter</param>
        internal void Delete(PromotionRuleDTO promotionRuleDTO)
        {
            log.LogMethodEntry(promotionRuleDTO);
            string query = @"DELETE  
                             FROM PromotionRule
                             WHERE PromotionRule.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", promotionRuleDTO.Id);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            promotionRuleDTO.AcceptChanges();
            log.LogMethodExit();
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="promotionRuleDTO">PromotionRuleDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>

        private void RefreshPromotionRuleDTO(PromotionRuleDTO promotionRuleDTO, DataTable dt)
        {
            log.LogMethodEntry(promotionRuleDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                promotionRuleDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                promotionRuleDTO.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                promotionRuleDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                promotionRuleDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                promotionRuleDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                promotionRuleDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                promotionRuleDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///  Inserts the record to the PromotionRule Table. 
        /// </summary>
        /// <param name="promotionRuleDTO">promotionRuleDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated PromotionRuleDTO</returns>
        public PromotionRuleDTO Insert(PromotionRuleDTO promotionRuleDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(promotionRuleDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[PromotionRule]
                            (
                            Promotion_Id,
                            CardTypeId,
                            site_id,
                            Guid,
                            CampaignId,
                            MasterEntityId,
                            MembershipID,
                            CreatedBy,
                            CreationDate,
                            LastUpdatedBy,
                            LastUpdateDate ,
                            IsActive
                            )
                            VALUES
                            (
                            @Promotion_Id,
                            @CardTypeId,
                            @site_id,
                            NEWID(),
                            @CampaignId,
                            @MasterEntityId,
                            @MembershipID,
                            @CreatedBy,
                            GETDATE(),
                            @LastUpdatedBy,
                            GETDATE()  , 
                            @IsActive
                            )
                            SELECT * FROM PromotionRule WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(promotionRuleDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPromotionRuleDTO(promotionRuleDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting PromotionRuleDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(promotionRuleDTO);
            return promotionRuleDTO;
        }

        /// <summary>
        /// Update the record in the PromotionRule Table. 
        /// </summary>
        /// <param name="promotionRuleDTO">promotionRuleDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated PromotionRuleDTO</returns>
        public PromotionRuleDTO Update(PromotionRuleDTO promotionRuleDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(promotionRuleDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[PromotionRule]
                             SET
                             Promotion_Id = @Promotion_Id,
                             CampaignId = @CampaignId,
                             MasterEntityId = @MasterEntityId,
                             MembershipID = @MembershipID,
                             LastUpdatedBy = @LastUpdatedBy,
                             LastUpdateDate = GETDATE()  ,
                             IsActive = @IsActive
                             WHERE Id = @Id
                            SELECT * FROM PromotionRule WHERE Id = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(promotionRuleDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPromotionRuleDTO(promotionRuleDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating PromotionRuleDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(promotionRuleDTO);
            return promotionRuleDTO;
        }

        internal List<PromotionRuleDTO> GetPromotionRuleDTOList(List<int> promotionIdList, bool activeRecords) //added
        {
            log.LogMethodEntry(promotionIdList);
            List<PromotionRuleDTO> promotionRuleDTOList = new List<PromotionRuleDTO>();
            string query = @"SELECT *
                            FROM PromotionRule, @promotionIdList List
                            WHERE Promotion_Id = List.Id ";
            if (activeRecords)
            {
                query += " AND (IsActive = 1 or IsActive is null)";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@promotionIdList", promotionIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                promotionRuleDTOList = table.Rows.Cast<DataRow>().Select(x => GetPromotionRuleDTO(x)).ToList();
            }
            log.LogMethodExit(promotionRuleDTOList);
            return promotionRuleDTOList;
        }

        /// <summary>
        /// Returns the List of PromotionRuleDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of PromotionRuleDTO</returns>
        public List<PromotionRuleDTO> GetPromotionRuleDTOList(List<KeyValuePair<PromotionRuleDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<PromotionRuleDTO> promotionRuleDTOList = new List<PromotionRuleDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<PromotionRuleDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == PromotionRuleDTO.SearchByParameters.PROMOTION_RULE_ID ||
                            searchParameter.Key == PromotionRuleDTO.SearchByParameters.PROMOTION_ID ||
                            searchParameter.Key == PromotionRuleDTO.SearchByParameters.CARD_TYPE_ID ||
                            searchParameter.Key == PromotionRuleDTO.SearchByParameters.MEMBERSHIP_ID ||
                            searchParameter.Key == PromotionRuleDTO.SearchByParameters.CAMPAIGN_ID ||
                            searchParameter.Key == PromotionRuleDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == PromotionRuleDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == PromotionRuleDTO.SearchByParameters.PROMOTION_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == PromotionRuleDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
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
                    PromotionRuleDTO promotionRuleDTO = GetPromotionRuleDTO(dataRow);
                    promotionRuleDTOList.Add(promotionRuleDTO);
                }
            }
            log.LogMethodExit(promotionRuleDTOList);
            return promotionRuleDTOList;
        }

       
    }
}
