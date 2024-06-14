/********************************************************************************************
 * Project Name - Promotions
 * Description  - Data Handler File for LoyaltyRedemptionRule
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks 
 *********************************************************************************************
 *2.80        10-June-2019   Divya A                 Created 
 *2.120.0      26-Mar-2021   Fiona                   Modified for container changes to add GetLoyaltyRedemptionRuleLastUpdateTime
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
    /// LoyaltyRedemptionRule Data Handler - Handles insert, update and selection of LoyaltyRedemptionRule objects
    /// </summary>
    class LoyaltyRedemptionRuleDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM LoyaltyRedemptionRule as lrr ";

        /// <summary>
        /// Dictionary for searching Parameters for the LoyaltyRedemptionRule object.
        /// </summary>
        private static readonly Dictionary<LoyaltyRedemptionRuleDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<LoyaltyRedemptionRuleDTO.SearchByParameters, string>
        {
            { LoyaltyRedemptionRuleDTO.SearchByParameters.REDEMPTION_RULE_ID,"lrr.RedemptionRuleId"},
            { LoyaltyRedemptionRuleDTO.SearchByParameters.LOYALTY_ATTR_ID,"lrr.LoyaltyAttributeId"},
            { LoyaltyRedemptionRuleDTO.SearchByParameters.ACTIVE_FLAG,"lrr.ActiveFlag"},
            { LoyaltyRedemptionRuleDTO.SearchByParameters.EXPIRY_DATE,"lrr.ExpiryDate"},
            { LoyaltyRedemptionRuleDTO.SearchByParameters.SITE_ID,"lrr.site_id"},
            { LoyaltyRedemptionRuleDTO.SearchByParameters.MASTER_ENTITY_ID,"lrr.MasterEntityId"}
        };

        /// <summary>
        /// Parameterized Constructor for LoyaltyRedemptionRuleDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        public LoyaltyRedemptionRuleDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating LoyaltyRedemptionRule Record.
        /// </summary>
        /// <param name="loyaltyRedemptionRuleDTO">LoyaltyRedemptionRuleDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(LoyaltyRedemptionRuleDTO loyaltyRedemptionRuleDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(loyaltyRedemptionRuleDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@RedemptionRuleId", loyaltyRedemptionRuleDTO.RedemptionRuleId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LoyaltyAttributeId", loyaltyRedemptionRuleDTO.LoyaltyAttributeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LoyaltyPoints", loyaltyRedemptionRuleDTO.LoyaltyPoints));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RedemptionValue", loyaltyRedemptionRuleDTO.RedemptionValue));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ExpiryDate", loyaltyRedemptionRuleDTO.ExpiryDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ActiveFlag", loyaltyRedemptionRuleDTO.ActiveFlag == true ? 'Y' : 'N'));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MinimumPoints", loyaltyRedemptionRuleDTO.MinimumPoints));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MaximumPoints", loyaltyRedemptionRuleDTO.MaximiumPoints));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MultiplesOnly", loyaltyRedemptionRuleDTO.MultiplesOnly));
            parameters.Add(dataAccessHandler.GetSQLParameter("@VirtualLoyaltyPoints", loyaltyRedemptionRuleDTO.VirtualPoints));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", loyaltyRedemptionRuleDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }
        /// <summary>
        /// Converts the Data row object to LoyaltyRedemptionRuleDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the object of LoyaltyRedemptionRuleDTO</returns>
        private LoyaltyRedemptionRuleDTO GetLoyaltyRedemptionRuleDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            LoyaltyRedemptionRuleDTO loyaltyRedemptionRuleDTO = new LoyaltyRedemptionRuleDTO(dataRow["RedemptionRuleId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["RedemptionRuleId"]),
                                                dataRow["LoyaltyAttributeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LoyaltyAttributeId"]),
                                                dataRow["LoyaltyPoints"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["LoyaltyPoints"]),
                                                dataRow["RedemptionValue"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["RedemptionValue"]),
                                                //dataRow["ExpiryDate"] == DBNull.Value ? DateTime.MaxValue : Convert.ToDateTime(dataRow["ExpiryDate"]),
                                                dataRow["ExpiryDate"] == DBNull.Value ? Convert.ToDateTime(null) : Convert.ToDateTime(dataRow["ExpiryDate"]),
                                                dataRow["ActiveFlag"] == DBNull.Value ? true : Convert.ToChar(dataRow["ActiveFlag"]) == 'Y' ? true : false,
                                                dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                dataRow["MinimumPoints"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["MinimumPoints"]),
                                                dataRow["MaximumPoints"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["MaximumPoints"]),
                                                dataRow["MultiplesOnly"] == DBNull.Value ? 'Y' : Convert.ToChar(dataRow["MultiplesOnly"]),
                                                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                                dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                dataRow["VirtualLoyaltyPoints"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["VirtualLoyaltyPoints"]));
            log.LogMethodExit(loyaltyRedemptionRuleDTO);
            return loyaltyRedemptionRuleDTO;
        }

        /// <summary>
        /// Gets the LoyaltyRedemptionRule data of passed LoyaltyRedemptionRule ID
        /// </summary>
        /// <param name="loyaltyRedemptionRuleId">loyaltyRedemptionRuleId is passed as parameter</param>
        /// <returns>Returns LoyaltyRedemptionRuleDTO</returns>
        public LoyaltyRedemptionRuleDTO GetLoyaltyRedemptionRuleDTO(int loyaltyRedemptionRuleId)
        {
            log.LogMethodEntry(loyaltyRedemptionRuleId);
            LoyaltyRedemptionRuleDTO result = null;
            string query = SELECT_QUERY + @" WHERE lrr.RedemptionRuleId = @RedemptionRuleId";
            SqlParameter parameter = new SqlParameter("@RedemptionRuleId", loyaltyRedemptionRuleId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetLoyaltyRedemptionRuleDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        ///  Deletes the LoyaltyRedemptionRule record
        /// </summary>
        /// <param name="loyaltyRedemptionRuleDTO">LoyaltyRedemptionRuleDTO is passed as parameter</param>
        internal void Delete(LoyaltyRedemptionRuleDTO loyaltyRedemptionRuleDTO)
        {
            log.LogMethodEntry(loyaltyRedemptionRuleDTO);
            string query = @"DELETE  
                             FROM LoyaltyRedemptionRule
                             WHERE LoyaltyRedemptionRule.RedemptionRuleId = @RedemptionRuleId";
            SqlParameter parameter = new SqlParameter("@RedemptionRuleId", loyaltyRedemptionRuleDTO.RedemptionRuleId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            loyaltyRedemptionRuleDTO.AcceptChanges();
            log.LogMethodExit();
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="loyaltyRedemptionRuleDTO">LoyaltyRedemptionRuleDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        private void RefreshLoyaltyRedemptionRuleDTO(LoyaltyRedemptionRuleDTO loyaltyRedemptionRuleDTO, DataTable dt)
        {
            log.LogMethodEntry(loyaltyRedemptionRuleDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                loyaltyRedemptionRuleDTO.RedemptionRuleId = Convert.ToInt32(dt.Rows[0]["RedemptionRuleId"]);
                loyaltyRedemptionRuleDTO.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                loyaltyRedemptionRuleDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                loyaltyRedemptionRuleDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                loyaltyRedemptionRuleDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                loyaltyRedemptionRuleDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                loyaltyRedemptionRuleDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///  Inserts the record to the LoyaltyRedemptionRule Table. 
        /// </summary>
        /// <param name="loyaltyRedemptionRuleDTO">LoyaltyRedemptionRuleDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated LoyaltyRedemptionRuleDTO</returns>
        public LoyaltyRedemptionRuleDTO Insert(LoyaltyRedemptionRuleDTO loyaltyRedemptionRuleDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(loyaltyRedemptionRuleDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[LoyaltyRedemptionRule]
                            (
                            LoyaltyAttributeId,
                            LoyaltyPoints,
                            RedemptionValue,
                            ExpiryDate,
                            ActiveFlag,
                            Guid,
                            site_id,
                            MinimumPoints,
                            MaximumPoints,
                            MultiplesOnly,
                            VirtualLoyaltyPoints,
                            MasterEntityId,
                            CreatedBy,
                            CreationDate,
                            LastUpdatedBy,
                            LastUpdateDate
                            )
                            VALUES
                            (
                            @LoyaltyAttributeId,
                            @LoyaltyPoints,
                            @RedemptionValue,
                            @ExpiryDate,
                            @ActiveFlag,
                            NEWID(),
                            @site_id,
                            @MinimumPoints,
                            @MaximumPoints,
                            @MultiplesOnly,
                            @VirtualLoyaltyPoints,
                            @MasterEntityId,
                            @CreatedBy,
                            GETDATE(),
                            @LastUpdatedBy,
                            GETDATE()
                            )
                            SELECT * FROM LoyaltyRedemptionRule WHERE RedemptionRuleId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(loyaltyRedemptionRuleDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshLoyaltyRedemptionRuleDTO(loyaltyRedemptionRuleDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting LoyaltyRedemptionRuleDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(loyaltyRedemptionRuleDTO);
            return loyaltyRedemptionRuleDTO;
        }

        /// <summary>
        /// Update the record in the LoyaltyRedemptionRule Table. 
        /// </summary>
        /// <param name="loyaltyRedemptionRuleDTO">LoyaltyRedemptionRuleDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated LoyaltyRedemptionRuleDTO</returns>
        public LoyaltyRedemptionRuleDTO Update(LoyaltyRedemptionRuleDTO loyaltyRedemptionRuleDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(loyaltyRedemptionRuleDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[LoyaltyRedemptionRule]
                             SET
                             LoyaltyAttributeId = @LoyaltyAttributeId,
                             LoyaltyPoints = @LoyaltyPoints,
                             RedemptionValue = @RedemptionValue,
                             ExpiryDate = @ExpiryDate,
                             ActiveFlag = @ActiveFlag,
                             MinimumPoints = @MinimumPoints,
                             MaximumPoints = @MaximumPoints,
                             MultiplesOnly = @MultiplesOnly,
                             VirtualLoyaltyPoints=@VirtualLoyaltyPoints,
                               MasterEntityId = @MasterEntityId,
                             LastUpdatedBy = @LastUpdatedBy,
                             LastUpdateDate = GETDATE()
                             WHERE RedemptionRuleId = @RedemptionRuleId
                            SELECT * FROM LoyaltyRedemptionRule WHERE RedemptionRuleId = @RedemptionRuleId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(loyaltyRedemptionRuleDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshLoyaltyRedemptionRuleDTO(loyaltyRedemptionRuleDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating LoyaltyRedemptionRuleDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(loyaltyRedemptionRuleDTO);
            return loyaltyRedemptionRuleDTO;
        }

        /// <summary>
        /// Returns the List of LoyaltyRedemptionRuleDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of LoyaltyRedemptionRuleDTO</returns>
        public List<LoyaltyRedemptionRuleDTO> GetLoyaltyRedemptionRuleDTOList(List<KeyValuePair<LoyaltyRedemptionRuleDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<LoyaltyRedemptionRuleDTO> loyaltyRedemptionRuleDTOList = new List<LoyaltyRedemptionRuleDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<LoyaltyRedemptionRuleDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == LoyaltyRedemptionRuleDTO.SearchByParameters.REDEMPTION_RULE_ID ||
                            searchParameter.Key == LoyaltyRedemptionRuleDTO.SearchByParameters.LOYALTY_ATTR_ID ||
                            searchParameter.Key == LoyaltyRedemptionRuleDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == LoyaltyRedemptionRuleDTO.SearchByParameters.EXPIRY_DATE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == LoyaltyRedemptionRuleDTO.SearchByParameters.ACTIVE_FLAG)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), (searchParameter.Value == "1" || searchParameter.Value == "Y" ? 'Y' : 'N')));
                        }
                        else if (searchParameter.Key == LoyaltyRedemptionRuleDTO.SearchByParameters.SITE_ID)
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
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    LoyaltyRedemptionRuleDTO loyaltyRedemptionRuleDTO = GetLoyaltyRedemptionRuleDTO(dataRow);
                    loyaltyRedemptionRuleDTOList.Add(loyaltyRedemptionRuleDTO);
                }
            }
            log.LogMethodExit(loyaltyRedemptionRuleDTOList);
            return loyaltyRedemptionRuleDTOList;
        }
        internal DateTime? GetLoyaltyRedemptionRuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"SELECT MAX(LastUpdateDate) LastUpdateDate 
                            FROM (
                            select max(LastUpdateDate) LastUpdateDate from LoyaltyRedemptionRule WHERE (site_id = @siteId or @siteId = -1)
                            )a";
            SqlParameter parameter = new SqlParameter("@siteId", siteId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, sqlTransaction);
            DateTime? result = null;
            if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["LastUpdateDate"] != DBNull.Value)
            {
                result = Convert.ToDateTime(dataTable.Rows[0]["LastUpdateDate"]);
            }
            log.LogMethodExit(result);
            return result;
        }

        internal DataTable GetEntitlementValue(decimal virtualPoints, bool isVirtualPoint)
        {
            try
            {
                log.LogMethodEntry(virtualPoints, isVirtualPoint);
                string query = string.Empty;
                if (isVirtualPoint)
                {
                    query = "select DBColumnName, attribute, convert(varchar, RedemptionValue) + ' for ' + convert(varchar, VirtualLoyaltyPoints) as \"Rule\", " +
                                @"RedemptionValue * (case MultiplesOnly 
                                                when 'Y' then Convert(int, ((case when @loyaltyPoints < isnull(MinimumPoints, 0) then 0 else @loyaltyPoints end) / (case when isnull(MinimumPoints, 1) = 0 then 1 else MinimumPoints end))) * (case when isnull(MinimumPoints, 1) = 0 then 1 else MinimumPoints end)
                                                else (case when @loyaltyPoints < isnull(MinimumPoints, 0) then 0 else @loyaltyPoints end) end)
                                                / case VirtualLoyaltyPoints when 0 then null else VirtualLoyaltyPoints end Redemption_value, " +
                                 "MinimumPoints \"Min Points\", MultiplesOnly \"Multiples Only\", RedemptionValue Rate, VirtualLoyaltyPoints " +
                                 "from LoyaltyRedemptionRule lrr, LoyaltyAttributes la " +
                                 "where lrr.LoyaltyAttributeId = la.LoyaltyAttributeId " +
                                 "and lrr.activeflag = 'Y'  and lrr.VirtualLoyaltyPoints IS NOT NULL " +
                                 "and (lrr.ExpiryDate is null or lrr.ExpiryDate >= getdate())";
                }
                else
                {
                    query = "select DBColumnName, attribute, convert(varchar, RedemptionValue) + ' for ' + convert(varchar, LoyaltyPoints) as \"Rule\", " +
                                 @"RedemptionValue * (case MultiplesOnly 
                                                when 'Y' then Convert(int, ((case when @loyaltyPoints < isnull(MinimumPoints, 0) then 0 else @loyaltyPoints end) / (case when isnull(MinimumPoints, 1) = 0 then 1 else MinimumPoints end))) * (case when isnull(MinimumPoints, 1) = 0 then 1 else MinimumPoints end)
                                                else (case when @loyaltyPoints < isnull(MinimumPoints, 0) then 0 else @loyaltyPoints end) end)
                                                / case LoyaltyPoints when 0 then null else LoyaltyPoints end Redemption_value, " +
                                  "MinimumPoints \"Min Points\", MultiplesOnly \"Multiples Only\", RedemptionValue Rate, LoyaltyPoints " +
                                  "from LoyaltyRedemptionRule lrr, LoyaltyAttributes la " +
                                  "where lrr.LoyaltyAttributeId = la.LoyaltyAttributeId " +
                                  "and lrr.activeflag = 'Y' " +
                                  "and (lrr.ExpiryDate is null or lrr.ExpiryDate >= getdate())";
                }

                SqlParameter[] selectUserParameters = new SqlParameter[1];
                selectUserParameters[0] = new SqlParameter("@loyaltyPoints", virtualPoints);
                DataTable redemptionRuleData = dataAccessHandler.executeSelectQuery(query, selectUserParameters, sqlTransaction);
                log.LogMethodExit();
                return redemptionRuleData;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }

        }
    }
}
