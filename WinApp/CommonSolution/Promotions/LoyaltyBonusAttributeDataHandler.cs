/********************************************************************************************
 * Project Name - Promotions
 * Description  - Data Handler File for LoyaltyBonusAttribute
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks 
 *********************************************************************************************
 *2.80      10-June-2019   Divya A                 Created 
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
    /// LoyaltyBonusAttributes Data Handler - Handles insert, update and selection of LoyaltyBonusAttributes objects
    /// </summary>
    class LoyaltyBonusAttributeDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM LoyaltyBonusAttributes as lba ";

        /// <summary>
        /// Dictionary for searching Parameters for the LoyaltyBonusAttributes object.
        /// </summary>
        private static readonly Dictionary<LoyaltyBonusAttributeDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<LoyaltyBonusAttributeDTO.SearchByParameters, string>
        {
            { LoyaltyBonusAttributeDTO.SearchByParameters.LOYALTY_BONUS_ID,"lba.LoyaltyBonusId"},
            { LoyaltyBonusAttributeDTO.SearchByParameters.LOYALTY_RULE_ID,"lba.LoyaltyRuleId"},
            { LoyaltyBonusAttributeDTO.SearchByParameters.LOYALTY_RULE_ID_LIST,"lba.LoyaltyRuleId"},
            { LoyaltyBonusAttributeDTO.SearchByParameters.LOYALTY_ATTRIBUTE_ID,"lba.LoyaltyAttributeId"},
            { LoyaltyBonusAttributeDTO.SearchByParameters.PERIOD_FROM,"lba.PeriodFrom"},
            { LoyaltyBonusAttributeDTO.SearchByParameters.PERIOD_TO,"lba.PeriodTo"},
            { LoyaltyBonusAttributeDTO.SearchByParameters.TIME_FROM,"lba.TimeFrom"},
            { LoyaltyBonusAttributeDTO.SearchByParameters.TIME_TO,"lba.TimeTo"},
            { LoyaltyBonusAttributeDTO.SearchByParameters.SITE_ID,"lba.site_id"},
            { LoyaltyBonusAttributeDTO.SearchByParameters.ACTIVE_FLAG,"lba.IsActive"},
            { LoyaltyBonusAttributeDTO.SearchByParameters.MASTER_ENTITY_ID,"lba.MasterEntityId"}
        };

        /// <summary>
        /// Parameterized Constructor for LoyaltyBonusAttributeDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        public LoyaltyBonusAttributeDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating LoyaltyBonusAttribute Record.
        /// </summary>
        /// <param name="loyaltyBonusAttributeDTO">LoyaltyBonusAttributeDTO object passed as a parameter.</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>SQL Parameters</returns>
        private List<SqlParameter> GetSQLParameters(LoyaltyBonusAttributeDTO loyaltyBonusAttributeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(loyaltyBonusAttributeDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@LoyaltyBonusId", loyaltyBonusAttributeDTO.LoyaltyBonusId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LoyaltyRuleId", loyaltyBonusAttributeDTO.LoyaltyRuleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LoyaltyAttributeId", loyaltyBonusAttributeDTO.LoyaltyAttributeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@BonusPercentage", loyaltyBonusAttributeDTO.BonusPercentage));
            parameters.Add(dataAccessHandler.GetSQLParameter("@BonusValue", loyaltyBonusAttributeDTO.BonusValue));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MinimumSaleAmount", loyaltyBonusAttributeDTO.MinimumSaleAmount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PeriodFrom", loyaltyBonusAttributeDTO.PeriodFrom));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PeriodTo", loyaltyBonusAttributeDTO.PeriodTo));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TimeFrom", loyaltyBonusAttributeDTO.TimeFrom));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TimeTo", loyaltyBonusAttributeDTO.TimeTo));
            parameters.Add(dataAccessHandler.GetSQLParameter("@NumberOfDays", loyaltyBonusAttributeDTO.NumberOfDays));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Monday", loyaltyBonusAttributeDTO.Monday));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Tuesday", loyaltyBonusAttributeDTO.Tuesday));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Wednesday", loyaltyBonusAttributeDTO.Wednesday));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Thursday", loyaltyBonusAttributeDTO.Thursday));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Friday", loyaltyBonusAttributeDTO.Friday));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Saturday", loyaltyBonusAttributeDTO.Saturday));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Sunday", loyaltyBonusAttributeDTO.Sunday));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ExtendOnReload", loyaltyBonusAttributeDTO.ExtendOnReload));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ValidAfterDays", loyaltyBonusAttributeDTO.ValidAfterDays));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ForMembershipOnly", loyaltyBonusAttributeDTO.ForMembershipOnly));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ApplicableElement", loyaltyBonusAttributeDTO.ApplicableElement));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", loyaltyBonusAttributeDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", loyaltyBonusAttributeDTO.ActiveFlag)); 
            log.LogMethodExit(parameters);
            return parameters;
        }
        /// <summary>
        /// Converts the Data row object to LoyaltyBonusAttributeDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the object of LoyaltyBonusAttributeDTO</returns>
        private LoyaltyBonusAttributeDTO GetLoyaltyBonusAttributeDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            LoyaltyBonusAttributeDTO loyaltyBonusRewardCriteriaDTO = new LoyaltyBonusAttributeDTO(dataRow["LoyaltyBonusId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LoyaltyBonusId"]),
                                                dataRow["LoyaltyRuleId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LoyaltyRuleId"]),
                                                dataRow["LoyaltyAttributeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LoyaltyAttributeId"]),
                                                dataRow["BonusPercentage"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["BonusPercentage"]),
                                                dataRow["BonusValue"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["BonusValue"]),
                                                dataRow["MinimumSaleAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["MinimumSaleAmount"]),
                                                dataRow["PeriodFrom"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["PeriodFrom"]),
                                                dataRow["PeriodTo"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["PeriodTo"]),
                                                dataRow["TimeFrom"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["TimeFrom"]),
                                                dataRow["TimeTo"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["TimeTo"]),
                                                dataRow["NumberOfDays"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["NumberOfDays"]),
                                                dataRow["Monday"] == DBNull.Value ? 'N' : Convert.ToChar(dataRow["Monday"]),
                                                dataRow["Tuesday"] == DBNull.Value ? 'N' : Convert.ToChar(dataRow["Tuesday"]),
                                                dataRow["Wednesday"] == DBNull.Value ? 'N' : Convert.ToChar(dataRow["Wednesday"]),
                                                dataRow["Thursday"] == DBNull.Value ? 'N' : Convert.ToChar(dataRow["Thursday"]),
                                                dataRow["Friday"] == DBNull.Value ? 'N' : Convert.ToChar(dataRow["Friday"]),
                                                dataRow["Saturday"] == DBNull.Value ? 'N' : Convert.ToChar(dataRow["Saturday"]),
                                                dataRow["Sunday"] == DBNull.Value ? 'N' : Convert.ToChar(dataRow["Sunday"]),
                                                dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                                dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                dataRow["ExtendOnReload"] == DBNull.Value ? 'N' : Convert.ToChar(dataRow["ExtendOnReload"]),
                                                dataRow["ValidAfterDays"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["ValidAfterDays"]),
                                                dataRow["ForMembershipOnly"] == DBNull.Value ? 'N' : Convert.ToChar(dataRow["ForMembershipOnly"]),
                                                dataRow["ApplicableElement"] == DBNull.Value ? 'A' : Convert.ToChar(dataRow["ApplicableElement"]),
                                                dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]));
            log.LogMethodExit(loyaltyBonusRewardCriteriaDTO);
            return loyaltyBonusRewardCriteriaDTO;
        }

        /// <summary>
        /// Gets the LoyaltyBonusPurchaseCriteria data of passed LoyaltyBonusAttribute ID
        /// </summary>
        /// <param name="loyaltyBonusAttributeId">loyaltyBonusAttributeId is passed as the parameter</param>
        /// <returns>Returns LoyaltyBonusAttributeDTO object</returns>
        public LoyaltyBonusAttributeDTO GetLoyaltyBonusAttributeDTO(int loyaltyBonusAttributeId)
        {
            log.LogMethodEntry(loyaltyBonusAttributeId);
            LoyaltyBonusAttributeDTO result = null;
            string query = SELECT_QUERY + @" WHERE lba.LoyaltyBonusId = @LoyaltyBonusId";
            SqlParameter parameter = new SqlParameter("@LoyaltyBonusId", loyaltyBonusAttributeId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetLoyaltyBonusAttributeDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        ///  Deletes the LoyaltyBonusAttribute record
        /// </summary>
        /// <param name="loyaltyBonusAttributeDTO">LoyaltyBonusAttributeDTO is passed as parameter</param>
        internal void Delete(LoyaltyBonusAttributeDTO loyaltyBonusAttributeDTO)
        {
            log.LogMethodEntry(loyaltyBonusAttributeDTO);
            string query = @"DELETE  
                             FROM LoyaltyBonusAttributes
                             WHERE LoyaltyBonusAttributes.LoyaltyBonusId = @Id";
            SqlParameter parameter = new SqlParameter("@Id", loyaltyBonusAttributeDTO.LoyaltyBonusId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            loyaltyBonusAttributeDTO.AcceptChanges();
            log.LogMethodExit();
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="loyaltyBonusAttributeDTO">LoyaltyBonusAttributeDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        private void RefreshLoyaltyBonusAttributeDTO(LoyaltyBonusAttributeDTO loyaltyBonusAttributeDTO, DataTable dt)
        {
            log.LogMethodEntry(loyaltyBonusAttributeDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                loyaltyBonusAttributeDTO.LoyaltyBonusId = Convert.ToInt32(dt.Rows[0]["LoyaltyBonusId"]);
                loyaltyBonusAttributeDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                loyaltyBonusAttributeDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                loyaltyBonusAttributeDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                loyaltyBonusAttributeDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                loyaltyBonusAttributeDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                loyaltyBonusAttributeDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///  Inserts the record to the LoyaltyBonusAttribute Table. 
        /// </summary>
        /// <param name="loyaltyBonusAttributeDTO">LoyaltyBonusAttributeDTO object passed as a parameter.</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated LoyaltyBonusAttributeDTO</returns>
        public LoyaltyBonusAttributeDTO Insert(LoyaltyBonusAttributeDTO loyaltyBonusAttributeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(loyaltyBonusAttributeDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[LoyaltyBonusAttributes]
                            (
                            LoyaltyRuleId,
                            LoyaltyAttributeId,
                            BonusPercentage,
                            BonusValue,
                            MinimumSaleAmount,
                            PeriodFrom,
                            PeriodTo,
                            TimeFrom,
                            TimeTo,
                            NumberOfDays,
                            Monday,
                            Tuesday,
                            Wednesday,
                            Thursday,
                            Friday,
                            Saturday,
                            Sunday,
                            LastUpdatedDate,
                            LastUpdatedBy,
                            Guid,
                            site_id,
                            ExtendOnReload,
                            ValidAfterDays,
                            MasterEntityId,
                            ForMembershipOnly,
                            ApplicableElement,
                            CreatedBy,
                            CreationDate , 
                            IsActive
                            )
                            VALUES
                            (
                            @LoyaltyRuleId,
                            @LoyaltyAttributeId,
                            @BonusPercentage,
                            @BonusValue,
                            @MinimumSaleAmount,
                            @PeriodFrom,
                            @PeriodTo,
                            @TimeFrom,
                            @TimeTo,
                            @NumberOfDays,
                            @Monday,
                            @Tuesday,
                            @Wednesday,
                            @Thursday,
                            @Friday,
                            @Saturday,
                            @Sunday,
                            GETDATE(),
                            @LastUpdatedBy,
                            NEWID(),
                            @site_id,
                            @ExtendOnReload,
                            @ValidAfterDays,
                            @MasterEntityId,
                            @ForMembershipOnly,
                            @ApplicableElement,
                            @CreatedBy,
                            GETDATE(),
                            @IsActive
                            )
                            SELECT * FROM LoyaltyBonusAttributes WHERE LoyaltyBonusId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(loyaltyBonusAttributeDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshLoyaltyBonusAttributeDTO(loyaltyBonusAttributeDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting LoyaltyBonusAttributeDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(loyaltyBonusAttributeDTO);
            return loyaltyBonusAttributeDTO;
        }

        /// <summary>
        /// Update the record in the LoyaltyBonusAttributes Table. 
        /// </summary>
        /// <param name="loyaltyBonusAttributeDTO">LoyaltyBonusAttributeDTO object passed as a parameter.</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated LoyaltyBonusAttributeDTO</returns>
        public LoyaltyBonusAttributeDTO Update(LoyaltyBonusAttributeDTO loyaltyBonusAttributeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(loyaltyBonusAttributeDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[LoyaltyBonusAttributes]
                             SET
                             LoyaltyRuleId = @LoyaltyRuleId,
                             LoyaltyAttributeId = @LoyaltyAttributeId,
                             BonusPercentage = @BonusPercentage,
                             BonusValue = @BonusValue,
                             MinimumSaleAmount = @MinimumSaleAmount,
                             PeriodFrom = @PeriodFrom,
                             PeriodTo = @PeriodTo,
                             TimeFrom = @TimeFrom,
                             TimeTo = @TimeTo,
                             NumberOfDays = @NumberOfDays,
                             Monday = @Monday,
                             Tuesday = @Tuesday,
                             Wednesday = @Wednesday,
                             Thursday = @Thursday,
                             Friday = @Friday,
                             Saturday = @Saturday,
                             Sunday = @Sunday,
                             LastUpdatedDate = GETDATE(),
                             LastUpdatedBy = @LastUpdatedBy,
                             ExtendOnReload = @ExtendOnReload,
                             ValidAfterDays = @ValidAfterDays,
                             MasterEntityId = @MasterEntityId,
                             ForMembershipOnly = @ForMembershipOnly,
                             ApplicableElement = @ApplicableElement,
                             IsActive = @IsActive
                             WHERE LoyaltyBonusId = @LoyaltyBonusId
                            SELECT * FROM LoyaltyBonusAttributes WHERE LoyaltyBonusId = @LoyaltyBonusId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(loyaltyBonusAttributeDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshLoyaltyBonusAttributeDTO(loyaltyBonusAttributeDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating LoyaltyBonusAttributeDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(loyaltyBonusAttributeDTO);
            return loyaltyBonusAttributeDTO;
        }

        /// <summary>
        /// Returns the List of LoyaltyBonusAttributeDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>List of LoyaltyBonusAttributeDTO</returns>
        public List<LoyaltyBonusAttributeDTO> GetLoyaltyBonusAttributeDTOList(List<KeyValuePair<LoyaltyBonusAttributeDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<LoyaltyBonusAttributeDTO> loyaltyBonusAttributeDTOList = new List<LoyaltyBonusAttributeDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<LoyaltyBonusAttributeDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == LoyaltyBonusAttributeDTO.SearchByParameters.LOYALTY_BONUS_ID ||
                            searchParameter.Key == LoyaltyBonusAttributeDTO.SearchByParameters.LOYALTY_RULE_ID ||
                            searchParameter.Key == LoyaltyBonusAttributeDTO.SearchByParameters.LOYALTY_ATTRIBUTE_ID ||
                            searchParameter.Key == LoyaltyBonusAttributeDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == LoyaltyBonusAttributeDTO.SearchByParameters.LOYALTY_RULE_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " in (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == LoyaltyBonusAttributeDTO.SearchByParameters.PERIOD_FROM)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + ">" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == LoyaltyBonusAttributeDTO.SearchByParameters.PERIOD_TO)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "<" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == LoyaltyBonusAttributeDTO.SearchByParameters.TIME_FROM)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + ">" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToDecimal(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == LoyaltyBonusAttributeDTO.SearchByParameters.TIME_TO)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "<" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToDecimal(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == LoyaltyBonusAttributeDTO.SearchByParameters.ACTIVE_FLAG)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else if (searchParameter.Key == LoyaltyBonusAttributeDTO.SearchByParameters.SITE_ID)
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
                    LoyaltyBonusAttributeDTO loyaltyBonusAttributeDTO = GetLoyaltyBonusAttributeDTO(dataRow);
                    loyaltyBonusAttributeDTOList.Add(loyaltyBonusAttributeDTO);
                }
            }
            log.LogMethodExit(loyaltyBonusAttributeDTOList);
            return loyaltyBonusAttributeDTOList;
        }
    }
}
