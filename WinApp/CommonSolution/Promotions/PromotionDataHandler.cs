/********************************************************************************************
 * Project Name - Promotions
 * Description  - Data Handler File for Promotion -PromotionDataHandler 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks 
 *********************************************************************************************
 *2.70        4-June-2019   Divya A                 Created 
 *2.70.0      18-Jul-2019   Mushahid Faizan         Modified ActiveFlag datatype from Char to bool.
 *2.80.       07-Apr-2020n  Mushahid Faizan         Modified 3 tier for Rest API changes.
 ********************************************************************************************/
using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Collections.Generic;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Promotions
{
    /// <summary>
    /// Promotion Data Handler - Handles insert, update and select of Promotion objects
    /// </summary>
   public class PromotionDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM Promotions as pm ";

        /// <summary>
        /// Dictionary for searching Parameters for the Promotions object.
        /// </summary>
        private static readonly Dictionary<PromotionDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<PromotionDTO.SearchByParameters, string>
        {
            { PromotionDTO.SearchByParameters.PROMOTION_ID,"pm.promotion_id"},
            { PromotionDTO.SearchByParameters.PROMOTION_NAME,"pm.promotion_name"},
            { PromotionDTO.SearchByParameters.PROMOTION_TYPE,"pm.PromotionType"},
            { PromotionDTO.SearchByParameters.TIME_FROM,"pm.time_from"},
            { PromotionDTO.SearchByParameters.TIME_TO,"pm.time_to"},
            { PromotionDTO.SearchByParameters.ACTIVE_FLAG,"pm.active_flag"},
            { PromotionDTO.SearchByParameters.RECUR_TYPE,"pm.RecurType"},
            { PromotionDTO.SearchByParameters.SITE_ID,"pm.site_id"},
            { PromotionDTO.SearchByParameters.MASTER_ENTITY_ID,"pm.MasterEntityId"}
        };

        /// <summary>
        /// Parameterized Constructor for PromotionDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction Object</param>
        public PromotionDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating Promotion Record.
        /// </summary>
        /// <param name="promotionDTO">PromotionDTO object is passed as Parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(PromotionDTO promotionDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(promotionDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@promotion_id", promotionDTO.PromotionId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@promotion_name", promotionDTO.PromotionName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@time_from", promotionDTO.TimeFrom));
            parameters.Add(dataAccessHandler.GetSQLParameter("@time_to", promotionDTO.TimeTo));
            parameters.Add(dataAccessHandler.GetSQLParameter("@active_flag", promotionDTO.ActiveFlag == true ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@recur_flag", promotionDTO.RecurFlag));
            parameters.Add(dataAccessHandler.GetSQLParameter("@recur_frequency", promotionDTO.RecurFrequency));
            parameters.Add(dataAccessHandler.GetSQLParameter("@recur_end_date", promotionDTO.RecurEndDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@InternetKey", promotionDTO.InternetKey));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PromotionType", promotionDTO.PromotionType));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RecurType", promotionDTO.RecurType));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", promotionDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Converts the Data row object to PromotionDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the object of PromotionDTO</returns>
        private PromotionDTO GetPromotionDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            PromotionDTO promotionDTO = new PromotionDTO(dataRow["promotion_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["promotion_id"]),
                                                dataRow["promotion_name"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["promotion_name"]),
                                                dataRow["time_from"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["time_from"]),
                                                dataRow["time_to"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["time_to"]),
                                                dataRow["active_flag"] == DBNull.Value ? false : (dataRow["active_flag"].ToString() == "Y" ? true : false),
                                                dataRow["recur_flag"] == DBNull.Value ? (char?)null : Convert.ToChar(dataRow["recur_flag"]),
                                                dataRow["recur_frequency"] == DBNull.Value ? (char?)null : Convert.ToChar(dataRow["recur_frequency"]),
                                                dataRow["recur_end_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["recur_end_date"]),
                                                dataRow["last_updated_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["last_updated_date"]),
                                                dataRow["last_updated_user"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["last_updated_user"]),
                                                dataRow["InternetKey"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["InternetKey"]),
                                                dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                dataRow["PromotionType"] == DBNull.Value ? (char?)null : Convert.ToChar(dataRow["PromotionType"]),
                                                dataRow["RecurType"] == DBNull.Value ? (char?)null : Convert.ToChar(dataRow["RecurType"]),
                                                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]));
            log.LogMethodExit(promotionDTO);
            return promotionDTO;
        }

        /// <summary>
        /// Converts the Data row object to PromotionViewDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the object of PromotionViewDTO</returns>
        private PromotionViewDTO GetPromotionViewDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            PromotionViewDTO promotionViewDTO = new PromotionViewDTO(dataRow["game_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["game_id"]),
                                                dataRow["game_profile_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["game_profile_id"]),
                                                dataRow["absolute_credits"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["absolute_credits"]),
                                                dataRow["discount_on_credits"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["discount_on_credits"]),
                                                dataRow["absolute_vip_credits"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["absolute_vip_credits"]),
                                                dataRow["discount_on_vip_credits"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["discount_on_vip_credits"]),
                                                dataRow["promotion_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["promotion_id"]),
                                                dataRow["promotion_detail_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["promotion_detail_id"]),
                                                dataRow["bonus_allowed"] == DBNull.Value ? "N" : Convert.ToString(dataRow["bonus_allowed"]),
                                                dataRow["courtesy_allowed"] == DBNull.Value ? "N" : Convert.ToString(dataRow["courtesy_allowed"]),
                                                dataRow["time_allowed"] == DBNull.Value ? "N" : Convert.ToString(dataRow["time_allowed"]),
                                                dataRow["ticket_allowed"] == DBNull.Value ? "N" : Convert.ToString(dataRow["ticket_allowed"]),
                                                dataRow["ThemeNumber"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ThemeNumber"]),
                                                dataRow["VisualizationThemeNumber"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["VisualizationThemeNumber"]));
            log.LogMethodExit(promotionViewDTO);
            return promotionViewDTO;
        }

        /// <summary>
        /// Gets the Promotion data of passed Promotion ID
        /// </summary>
        /// <param name="promotionId">promotionId is passed as Parameter</param>
        /// <returns>Returns PromotionDTO</returns>
        public PromotionDTO GetPromotionDTO(int promotionId)
        {
            log.LogMethodEntry(promotionId);
            PromotionDTO result = null;
            string query = SELECT_QUERY + @" WHERE pm.promotion_id = @promotion_id";
            SqlParameter parameter = new SqlParameter("@promotion_id", promotionId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetPromotionDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        ///  Deletes the Promotion record
        /// </summary>
        /// <param name="promotionDTO">PromotionDTO is passed as parameter</param>
        internal void Delete(PromotionDTO promotionDTO)
        {
            log.LogMethodEntry(promotionDTO);
            string query = @"DELETE  
                             FROM Promotions
                             WHERE Promotions.promotion_id = @promotion_id";
            SqlParameter parameter = new SqlParameter("@promotion_id", promotionDTO.PromotionId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            promotionDTO.AcceptChanges();
            log.LogMethodExit();
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="promotionDTO">PromotionDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        private void RefreshPromotionDTO(PromotionDTO promotionDTO, DataTable dt)
        {
            log.LogMethodEntry(promotionDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                promotionDTO.PromotionId = Convert.ToInt32(dt.Rows[0]["promotion_id"]);
                promotionDTO.LastUpdatedDate = dataRow["last_updated_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["last_updated_date"]);
                promotionDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                promotionDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                promotionDTO.LastUpdatedBy = dataRow["last_updated_user"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["last_updated_user"]);
                promotionDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                promotionDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///  Inserts the record to the Promotions Table. 
        /// </summary>
        /// <param name="promotionDTO">PromotionDTO object is passed as Parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated PromotionDTO</returns>
        public PromotionDTO Insert(PromotionDTO promotionDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(promotionDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[Promotions]
                            (
                            promotion_name,
                            time_from,
                            time_to,
                            active_flag,
                            recur_flag,
                            recur_frequency,
                            recur_end_date,
                            last_updated_date,
                            last_updated_user,
                            InternetKey,
                            Guid,
                            site_id,
                            PromotionType,
                            RecurType,
                            MasterEntityId,
                            CreatedBy,
                            CreationDate
                            )
                            VALUES
                            (
                            @promotion_name,
                            @time_from,
                            @time_to,
                            @active_flag,
                            @recur_flag,
                            @recur_frequency,
                            @recur_end_date,
                            GETDATE(),
                            @LastUpdatedBy,
                            @InternetKey,
                            NEWID(),
                            @site_id,
                            @PromotionType,
                            @RecurType,
                            @MasterEntityId,
                            @CreatedBy,
                            GETDATE()                      
                            )
                            SELECT * FROM Promotions WHERE promotion_id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(promotionDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPromotionDTO(promotionDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting PromotionDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(promotionDTO);
            return promotionDTO;
        }

        /// <summary>
        /// Update the record in the Promotions Table. 
        /// </summary>
        /// <param name="promotionDTO">PromotionDTO object is passed as Parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated PromotionDTO</returns>
        public PromotionDTO Update(PromotionDTO promotionDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(promotionDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[Promotions]
                             SET
                             promotion_name = @promotion_name,
                             time_from = @time_from,
                             time_to = @time_to,
                             active_flag = @active_flag,
                             recur_flag = @recur_flag,
                             recur_frequency = @recur_frequency,
                             recur_end_date = @recur_end_date,
                             last_updated_date = GETDATE(),
                             last_updated_user = @LastUpdatedBy,
                             InternetKey = @InternetKey,
                             PromotionType = @PromotionType,
                             RecurType = @RecurType,
                             MasterEntityId = @MasterEntityId        
                             WHERE promotion_id = @promotion_id
                            SELECT * FROM Promotions WHERE promotion_id = @promotion_id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(promotionDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPromotionDTO(promotionDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating PromotionDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(promotionDTO);
            return promotionDTO;
        }

        /// <summary>
        /// Returns the List of PromotionDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of PromotionDTO</returns>
        public List<PromotionDTO> GetPromotionDTOList(List<KeyValuePair<PromotionDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<PromotionDTO> promotionDTOList = new List<PromotionDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<PromotionDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == PromotionDTO.SearchByParameters.PROMOTION_ID ||
                            searchParameter.Key == PromotionDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == PromotionDTO.SearchByParameters.PROMOTION_NAME)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == PromotionDTO.SearchByParameters.TIME_FROM)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + ">" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == PromotionDTO.SearchByParameters.TIME_TO)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "<" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == PromotionDTO.SearchByParameters.PROMOTION_TYPE ||
                                searchParameter.Key == PromotionDTO.SearchByParameters.RECUR_TYPE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == PromotionDTO.SearchByParameters.ACTIVE_FLAG)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y')= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), (searchParameter.Value == "1" || searchParameter.Value == "Y" ? 'Y' : 'N')));
                        }
                        else if (searchParameter.Key == PromotionDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
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
                selectQuery = selectQuery + query + " order by time_from desc";
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    PromotionDTO promotionDTO = GetPromotionDTO(dataRow);
                    promotionDTOList.Add(promotionDTO);
                }
            }
            log.LogMethodExit(promotionDTOList);
            return promotionDTOList;
        }

        internal DateTime? GetPromotionModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"SELECT MAX(LastUpdatedDate) LastUpdatedDate 
                            FROM (
                            select max(Last_Updated_Date) LastUpdatedDate from promotions WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastUpdateDate) LastUpdatedDate from PromotionRule WHERE (site_id = @siteId or @siteId = -1)
							union all
                            select max(Last_Updated_Date) LastUpdatedDate from promotion_detail WHERE (site_id = @siteId or @siteId = -1)
							union all
                            select max(LastUpdateDate) LastUpdatedDate from PromotionExclusionDates WHERE (site_id = @siteId or @siteId = -1)
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

        /// <summary>
        /// Returns the PromotionViewDTO based on the parameters.
        /// </summary>
        /// <returns>Returns the of PromotionDTO</returns>
        public PromotionViewDTO GetGamePromotionDetailViewDTO(int membershipId, int gameId, int gameProfileId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(membershipId, gameId, gameProfileId, sqlTransaction);
            PromotionViewDTO promotionViewDTO = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = @"select v.*, rt.ThemeNumber, rtv.ThemeNumber VisualizationThemeNumber, case when @membershipId < 0 then r.MembershipId else case when r.MembershipId = @membershipId then -2 else isnull(r.MembershipId, -1) end end sort1, 1 sort2 
                                from PromotionView v
                                left outer join PromotionRule r
                                    on r.promotion_id = v.promotion_id
                                left outer join Theme rt
                                    on rt.Id = v.ThemeId
                                left outer join Theme rtv
                                    on rtv.Id = v.VisualizationThemeId
                              where v.game_id = @game_id
                              union all
                              select v.*, rt.ThemeNumber, rtv.ThemeNumber VisualizationThemeNumber, case when @membershipId < 0 then r.MembershipId else case when r.MembershipId = @membershipId then -2 else isnull(r.MembershipId, -1) end end, 2 
                                from PromotionView v
                                left outer join PromotionRule r
                                    on r.promotion_id = v.promotion_id
                                left outer join Theme rt
                                    on rt.Id = v.ThemeId
                                left outer join Theme rtv
                                    on rtv.Id = v.VisualizationThemeId
                              where v.game_profile_id = @game_profile_id
                              union all
                              select v.*, rt.ThemeNumber, rtv.ThemeNumber VisualizationThemeNumber, case when @membershipId < 0 then r.MembershipId else case when r.MembershipId = @membershipId then -2 else isnull(r.MembershipId, -1) end end, 3 
                                from PromotionView v
                                left outer join PromotionRule r
                                    on r.promotion_id = v.promotion_id
                                left outer join Theme rt
                                    on rt.Id = v.ThemeId
                                left outer join Theme rtv
                                    on rtv.Id = v.VisualizationThemeId
                              where v.game_id is null 
                                and v.game_profile_id is null
                                and v.PromotionType = 'G'
                                order by sort1, sort2";
            parameters.Add(new SqlParameter("@membershipId", membershipId));
            parameters.Add(new SqlParameter("@game_id", gameId));
            parameters.Add(new SqlParameter("@game_profile_id", gameProfileId));
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                promotionViewDTO = GetPromotionViewDTO(dataTable.Rows[0]);
                //promotionViewDTO.Add(promotionViewDTO);
            }
            log.LogMethodExit(promotionViewDTO);
            return promotionViewDTO;
        }

        /// <summary>
        /// Returns the List of PromotionViewDTOList based on the parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of PromotionDTO</returns>
        public List<PromotionViewDTO> GetGamePromotionDetailViewDTOList(int membershipId, string machineIdList)
        {
            log.LogMethodEntry(membershipId, machineIdList, sqlTransaction);
            List<PromotionViewDTO> promotionViewDTOList = new List<PromotionViewDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = @"select v.*, rt.ThemeNumber, rtv.ThemeNumber VisualizationThemeNumber, case when @membershipId < 0 then r.MembershipId else case when r.MembershipId = @membershipId then -2 else isnull(r.MembershipId, -1) end end sort1, 1 sort2 
                                from PromotionView v
                                left outer join PromotionRule r
                                    on r.promotion_id = v.promotion_id
                                left outer join Theme rt
                                    on rt.Id = v.ThemeId
                                left outer join Theme rtv
                                    on rtv.Id = v.VisualizationThemeId
                              where v.game_id in (select game_id from machines where machine_id in " + machineIdList + @")
                              union all
                              select v.*, rt.ThemeNumber, rtv.ThemeNumber VisualizationThemeNumber, case when @membershipId < 0 then r.MembershipId else case when r.MembershipId = @membershipId then -2 else isnull(r.MembershipId, -1) end end, 2 
                                from PromotionView v
                                left outer join PromotionRule r
                                    on r.promotion_id = v.promotion_id
                                left outer join Theme rt
                                    on rt.Id = v.ThemeId
                                left outer join Theme rtv
                                    on rtv.Id = v.VisualizationThemeId
                              where v.game_profile_id in (select g.game_profile_id from games g, machines m
                                                           where g.game_id = m.game_id 
                                                             and m.machine_id in " + machineIdList + @")
                              union all
                              select v.*, rt.ThemeNumber, rtv.ThemeNumber VisualizationThemeNumber, case when @membershipId < 0 then r.MembershipId else case when r.MembershipId = @membershipId then -2 else isnull(r.MembershipId, -1) end end, 3 
                                from PromotionView v
                                left outer join PromotionRule r
                                    on r.promotion_id = v.promotion_id
                                left outer join Theme rt
                                    on rt.Id = v.ThemeId
                                left outer join Theme rtv
                                    on rtv.Id = v.VisualizationThemeId
                              where v.game_id is null 
                                and v.game_profile_id is null
                                and v.PromotionType = 'G'
                                order by sort1, sort2";
            parameters.Add(new SqlParameter("@membershipId", membershipId));
            parameters.Add(new SqlParameter("@machineIdList", machineIdList));
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    PromotionViewDTO promotionViewDTO = GetPromotionViewDTO(dataRow);
                    promotionViewDTOList.Add(promotionViewDTO);
                }
            }
            log.LogMethodExit(promotionViewDTOList);
            return promotionViewDTOList;
        }
    }
}