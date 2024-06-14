/********************************************************************************************
 * Project Name - Promotions
 * Description  - Data Handler File for PromotionDetail
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks 
 *********************************************************************************************
 *2.70        10-June-2019   Divya A                 Created 
 *2.80     08-Apr-2020     Mushahid Faizan           Modified : 3 tier changes for Rest API.
 *2.80     24-Apr-2020     Girish Kundar             Modified : Removed the search by PromotionIdList method .
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
    /// PromotionDetail Data Handler - Handles insert, update and select of PromotionDetail objects
    /// </summary>
    class PromotionDetailDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM promotion_detail as pd ";

        /// <summary>
        /// Dictionary for searching Parameters for the PromotionDetail object.
        /// </summary>
        private static readonly Dictionary<PromotionDetailDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<PromotionDetailDTO.SearchByParameters, string>
        {
            { PromotionDetailDTO.SearchByParameters.PROMOTION_DETAIL_ID,"pd.promotion_detail_id"},
            { PromotionDetailDTO.SearchByParameters.PROMOTION_ID,"pd.promotion_id"},
            { PromotionDetailDTO.SearchByParameters.PROMOTION_ID_LIST,"pd.promotion_id"},
            { PromotionDetailDTO.SearchByParameters.GAME_ID,"pd.game_id"},
            { PromotionDetailDTO.SearchByParameters.GAME_PROFILE_ID,"pd.game_profile_id"},
            { PromotionDetailDTO.SearchByParameters.CATEGORY_ID ,"pd.CategoryId"},
            { PromotionDetailDTO.SearchByParameters.THEME_ID,"pd.ThemeId"},
            { PromotionDetailDTO.SearchByParameters.VISUALIZATION_THEME_ID,"pd.VisualizationThemeId"},
            { PromotionDetailDTO.SearchByParameters.PRODUCT_ID,"pd.product_id"},
            { PromotionDetailDTO.SearchByParameters.BONUS_ALLOWED,"pd.bonus_allowed"},
            { PromotionDetailDTO.SearchByParameters.IS_ACTIVE,"pd.IsActive"},
            { PromotionDetailDTO.SearchByParameters.COURTESY_ALLOWED,"pd.courtesy_allowed"},
            { PromotionDetailDTO.SearchByParameters.TICKETS_ALLOWED,"pd.ticket_allowed"},
            { PromotionDetailDTO.SearchByParameters.SITE_ID,"pd.site_id"},
            { PromotionDetailDTO.SearchByParameters.GUID,"pd.Guid"},
            { PromotionDetailDTO.SearchByParameters.MASTER_ENTITY_ID,"pd.MasterEntityId"}
        };

        /// <summary>
        /// Parameterized Constructor for PromotionDetailDataHandler.
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public PromotionDetailDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating PromotionDetail Record.
        /// </summary>
        /// <param name="promotionDetailDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        private List<SqlParameter> GetSQLParameters(PromotionDetailDTO promotionDetailDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(promotionDetailDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@promotion_detail_id", promotionDetailDTO.PromotionDetailId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@promotion_id", promotionDetailDTO.PromotionId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@game_id", promotionDetailDTO.GameId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@game_profile_id", promotionDetailDTO.GameprofileId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@absolute_credits", promotionDetailDTO.AbsoluteCredits));
            parameters.Add(dataAccessHandler.GetSQLParameter("@absolute_vip_credits", promotionDetailDTO.AbsoluteVIPCredits));
            parameters.Add(dataAccessHandler.GetSQLParameter("@discount_on_credits", promotionDetailDTO.DiscountOnCredits));
            parameters.Add(dataAccessHandler.GetSQLParameter("@discount_on_vip_credits", promotionDetailDTO.DiscountOnVIPCredits));
            parameters.Add(dataAccessHandler.GetSQLParameter("@bonus_allowed", promotionDetailDTO.BonusAllowed));
            parameters.Add(dataAccessHandler.GetSQLParameter("@courtesy_allowed", promotionDetailDTO.CourtesyAllowed));
            parameters.Add(dataAccessHandler.GetSQLParameter("@time_allowed", promotionDetailDTO.TimeAllowed));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ticket_allowed", promotionDetailDTO.TicketAllowed));
            parameters.Add(dataAccessHandler.GetSQLParameter("@last_updated_user", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@InternetKey", promotionDetailDTO.InternetKey));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Guid", promotionDetailDTO.Guid));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@product_id", promotionDetailDTO.ProductId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CategoryId", promotionDetailDTO.CategoryId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", promotionDetailDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ThemeId", promotionDetailDTO.ThemeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DiscountAmount", promotionDetailDTO.DiscountAmount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@VisualizationThemeId", promotionDetailDTO.VisualizationThemeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", promotionDetailDTO.IsActive));

            log.LogMethodExit(parameters);
            return parameters;
        }

        private PromotionDetailDTO GetPromotionDetailDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            PromotionDetailDTO promotionDetailDTO = new PromotionDetailDTO(dataRow["promotion_detail_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["promotion_detail_id"]),
                                                        dataRow["promotion_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["promotion_id"]),
                                                        dataRow["game_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["game_id"]),
                                                        dataRow["game_profile_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["game_profile_id"]),
                                                        dataRow["absolute_credits"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["absolute_credits"]),
                                                        dataRow["absolute_vip_credits"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["absolute_vip_credits"]),
                                                        dataRow["discount_on_credits"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["discount_on_credits"]),
                                                        dataRow["discount_on_vip_credits"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["discount_on_vip_credits"]),
                                                        dataRow["bonus_allowed"] == DBNull.Value ? (char?)null : Convert.ToChar(dataRow["bonus_allowed"]),
                                                        dataRow["courtesy_allowed"] == DBNull.Value ? (char?)null : Convert.ToChar(dataRow["courtesy_allowed"]),
                                                        dataRow["time_allowed"] == DBNull.Value ? (char?)null : Convert.ToChar(dataRow["time_allowed"]),
                                                        dataRow["ticket_allowed"] == DBNull.Value ? (char?)null : Convert.ToChar(dataRow["ticket_allowed"]),
                                                        dataRow["last_updated_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["last_updated_date"]),
                                                        dataRow["last_updated_user"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["last_updated_user"]),
                                                        dataRow["InternetKey"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["InternetKey"]),
                                                        dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                        dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                        dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                        dataRow["product_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["product_id"]),
                                                        dataRow["CategoryId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CategoryId"]),
                                                        dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                        dataRow["ThemeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ThemeId"]),
                                                        dataRow["DiscountAmount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["DiscountAmount"]),
                                                        dataRow["VisualizationThemeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["VisualizationThemeId"]),
                                                        dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                        dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                        dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]));
            log.LogMethodExit(promotionDetailDTO);
            return promotionDetailDTO;
        }

        /// <summary>
        /// Gets the PromotionDetail data of passed PromotionDetail ID
        /// </summary>
        /// <param name="promotionDetailId"></param>
        /// <returns>Returns PromotionDetailDTO</returns>
        public PromotionDetailDTO GetPromotionDetailDTO(int promotionDetailId)
        {
            log.LogMethodEntry(promotionDetailId);
            PromotionDetailDTO result = null;
            string query = SELECT_QUERY + @" WHERE pd.promotion_detail_id = @promotion_detail_id";
            SqlParameter parameter = new SqlParameter("@promotion_detail_id", promotionDetailId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetPromotionDetailDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        ///  Deletes the PromotionDetail record
        /// </summary>
        /// <param name="promotionDetailDTO">PromotionDetailDTO is passed as parameter</param>
        internal void Delete(PromotionDetailDTO promotionDetailDTO)
        {
            log.LogMethodEntry(promotionDetailDTO);
            string query = @"DELETE  
                             FROM promotion_detail
                             WHERE promotion_detail.promotion_detail_id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", promotionDetailDTO.PromotionDetailId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            promotionDetailDTO.AcceptChanges();
            log.LogMethodExit();
        }

        private void RefreshPromotionDetailDTO(PromotionDetailDTO promotionDetailDTO, DataTable dt)
        {
            log.LogMethodEntry(promotionDetailDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                promotionDetailDTO.PromotionDetailId = Convert.ToInt32(dt.Rows[0]["promotion_detail_id"]);
                promotionDetailDTO.LastUpdatedDate = dataRow["last_updated_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["last_updated_date"]);
                promotionDetailDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                promotionDetailDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                promotionDetailDTO.LastUpdatedBy = dataRow["last_updated_user"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["last_updated_user"]);
                promotionDetailDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                promotionDetailDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///  Inserts the record to the PromotionDetail Table. 
        /// </summary>
        /// <param name="promotionDetailDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns>Returns updated PromotionDetailDTO</returns>
        public PromotionDetailDTO Insert(PromotionDetailDTO promotionDetailDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(promotionDetailDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[promotion_detail]
                            (
                            promotion_id,
                            game_id,
                            game_profile_id,
                            absolute_credits,
                            absolute_vip_credits,
                            discount_on_credits,
                            discount_on_vip_credits,
                            bonus_allowed,
                            courtesy_allowed,
                            time_allowed,
                            ticket_allowed,
                            last_updated_date,
                            last_updated_user,
                            InternetKey,
                            Guid,
                            site_id,
                            product_id,
                            CategoryId,
                            MasterEntityId,
                            ThemeId,
                            DiscountAmount,
                            VisualizationThemeId,
                            CreatedBy,
                            CreationDate, IsActive
                            )
                            VALUES
                            (
                            @promotion_id,
                            @game_id,
                            @game_profile_id,
                            @absolute_credits,
                            @absolute_vip_credits,
                            @discount_on_credits,
                            @discount_on_vip_credits,
                            @bonus_allowed,
                            @courtesy_allowed,
                            @time_allowed,
                            @ticket_allowed,
                            GETDATE(),
                            @last_updated_user,
                            @InternetKey,
                            NEWID(),
                            @site_id,
                            @product_id,
                            @CategoryId,
                            @MasterEntityId,
                            @ThemeId,
                            @DiscountAmount,
                            @VisualizationThemeId,
                            @CreatedBy,
                            GETDATE() ,@IsActive
                            )
                            SELECT * FROM promotion_detail WHERE promotion_detail_id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(promotionDetailDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPromotionDetailDTO(promotionDetailDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting PromotionDetailDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(promotionDetailDTO);
            return promotionDetailDTO;
        }

        /// <summary>
        /// Update the record in the PromotionDetail Table. 
        /// </summary>
        /// <param name="promotionDetailDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns>Returns updated PromotionDetailDTO</returns>
        public PromotionDetailDTO Update(PromotionDetailDTO promotionDetailDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(promotionDetailDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[promotion_detail]
                             SET
                             promotion_id = @promotion_id,
                             game_id = @game_id,
                             game_profile_id = @game_profile_id,
                             absolute_credits = @absolute_credits,
                             absolute_vip_credits = @absolute_vip_credits,
                             discount_on_credits = @discount_on_credits,
                             discount_on_vip_credits = @discount_on_vip_credits,
                             bonus_allowed = @bonus_allowed,
                             courtesy_allowed = @courtesy_allowed,
                             time_allowed = @time_allowed,
                             ticket_allowed = @ticket_allowed,
                             last_updated_date = GETDATE(),
                             last_updated_user = @last_updated_user,
                             InternetKey = @InternetKey,
                             product_id = @product_id,
                             CategoryId = @CategoryId,
                             MasterEntityId = @MasterEntityId,
                             ThemeId = @ThemeId,
                             DiscountAmount = @DiscountAmount,
                             VisualizationThemeId = @VisualizationThemeId,
                             IsActive = @IsActive
                             WHERE promotion_detail_id = @promotion_detail_id
                            SELECT * FROM promotion_detail WHERE promotion_detail_id = @promotion_detail_id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(promotionDetailDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPromotionDetailDTO(promotionDetailDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating PromotionDetailDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(promotionDetailDTO);
            return promotionDetailDTO;
        }


        internal List<PromotionDetailDTO> GetPromotionDetailDTOList(List<int> promotionIdList, bool activeRecords) //added
        {
            log.LogMethodEntry(promotionIdList);
            List<PromotionDetailDTO> promotionDetailDTOList = new List<PromotionDetailDTO>();
            string query = @"SELECT *
                            FROM promotion_detail, @promotionIdList List
                            WHERE promotion_id = List.Id ";
            if (activeRecords)
            {
                query += " AND (IsActive = 1 or IsActive is null)";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@promotionIdList", promotionIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                promotionDetailDTOList = table.Rows.Cast<DataRow>().Select(x => GetPromotionDetailDTO(x)).ToList();
            }
            log.LogMethodExit(promotionDetailDTOList);
            return promotionDetailDTOList;
        }

        /// <summary>
        /// Returns the List of PromotionDetailDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the :List of PromotionDetailDTO </returns>
        public List<PromotionDetailDTO> GetPromotionDetailDTOList(List<KeyValuePair<PromotionDetailDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<PromotionDetailDTO> promotionDetailDTOList = new List<PromotionDetailDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<PromotionDetailDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == PromotionDetailDTO.SearchByParameters.PROMOTION_DETAIL_ID ||
                            searchParameter.Key == PromotionDetailDTO.SearchByParameters.PROMOTION_ID ||
                            searchParameter.Key == PromotionDetailDTO.SearchByParameters.GAME_ID ||
                            searchParameter.Key == PromotionDetailDTO.SearchByParameters.GAME_PROFILE_ID ||
                            searchParameter.Key == PromotionDetailDTO.SearchByParameters.CATEGORY_ID ||
                            searchParameter.Key == PromotionDetailDTO.SearchByParameters.PRODUCT_ID ||
                            searchParameter.Key == PromotionDetailDTO.SearchByParameters.THEME_ID ||
                            searchParameter.Key == PromotionDetailDTO.SearchByParameters.VISUALIZATION_THEME_ID ||
                            searchParameter.Key == PromotionDetailDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == PromotionDetailDTO.SearchByParameters.BONUS_ALLOWED ||
                                 searchParameter.Key == PromotionDetailDTO.SearchByParameters.COURTESY_ALLOWED ||
                                 searchParameter.Key == PromotionDetailDTO.SearchByParameters.TICKETS_ALLOWED)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToChar(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == PromotionDetailDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == PromotionDetailDTO.SearchByParameters.GUID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == PromotionDetailDTO.SearchByParameters.PROMOTION_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == PromotionDetailDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), (searchParameter.Value == "1" || searchParameter.Value == "Y" ? true : false)));
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
                    PromotionDetailDTO promotionDetailDTO = GetPromotionDetailDTO(dataRow);
                    promotionDetailDTOList.Add(promotionDetailDTO);
                }
            }
            log.LogMethodExit(promotionDetailDTOList);
            return promotionDetailDTOList;
        }

    }
}
