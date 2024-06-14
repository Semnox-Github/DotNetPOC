/********************************************************************************************
 * Project Name - Promotions
 * Description  - Data Handler File for LoyaltyRuleTrigger
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks 
 *********************************************************************************************
 *2.70        10-June-2019   Divya A                 Created 
 *2.70.3      06-Feb-2020    Girish Kundar           Modified : As per the 3 tier standard
 *2.80        20-Dec-2019   Vikas Dwivedi           Modified GetSQLParameters()
 ********************************************************************************************/
using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Promotions
{
    /// <summary>
    /// LoyaltyRuleTriggers Data Handler - Handles insert, update and select of LoyaltyRuleTriggers objects
    /// </summary>
    public class LoyaltyRuleTriggerDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM LoyaltyRuleTriggers as lrt ";

        /// <summary>
        /// Dictionary for searching Parameters for the LoyaltyRuleTrigger object.
        /// </summary>
        private static readonly Dictionary<LoyaltyRuleTriggerDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<LoyaltyRuleTriggerDTO.SearchByParameters, string>
        {
            { LoyaltyRuleTriggerDTO.SearchByParameters.LOYALTY_RULE_TRIG_ID,"lrt.LoyaltyRuleTriggerId"},
            { LoyaltyRuleTriggerDTO.SearchByParameters.LOYALTY_RULE_ID,"lrt.LoyaltyRuleId"},
            { LoyaltyRuleTriggerDTO.SearchByParameters.LOYALTY_RULE_ID_LIST,"lrt.LoyaltyRuleId"},
            { LoyaltyRuleTriggerDTO.SearchByParameters.APPLICABLE_PRODUCT_ID,"lrt.ApplicableProductId"},
            { LoyaltyRuleTriggerDTO.SearchByParameters.APPLICABLE_PRODUCT_TYPE_ID,"lrt.ApplicableProductTypeId"},
            { LoyaltyRuleTriggerDTO.SearchByParameters.APPLICABLE_GAME_ID,"lrt.ApplicableGameId"},
            { LoyaltyRuleTriggerDTO.SearchByParameters.APPLICABLE_GAME_PROFILE_ID,"lrt.ApplicableGameProfileId"},
            { LoyaltyRuleTriggerDTO.SearchByParameters.POS_TYPE_ID,"lrt.ApplicablePOSTypeId"},
            { LoyaltyRuleTriggerDTO.SearchByParameters.IS_ACTIVE,"lrt.IsActive"},
            { LoyaltyRuleTriggerDTO.SearchByParameters.CATEGORY_ID,"lrt.CategoryId"},
            { LoyaltyRuleTriggerDTO.SearchByParameters.EXCLUDE_FLAG,"lrt.ExcludeFlag"},
            { LoyaltyRuleTriggerDTO.SearchByParameters.SITE_ID,"lrt.site_id"},
            { LoyaltyRuleTriggerDTO.SearchByParameters.MASTER_ENTITY_ID,"lrt.MasterEntityId"}
        };

        /// <summary>
        /// Parameterized Constructor for LoyaltyRuleTriggerDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        public LoyaltyRuleTriggerDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating LoyaltyRuleTrigger Record.
        /// </summary>
        /// <param name="loyaltyRuleTriggerDTO">LoyaltyRuleTriggerDTO is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(LoyaltyRuleTriggerDTO loyaltyRuleTriggerDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(loyaltyRuleTriggerDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@LoyaltyRuleTriggerId", loyaltyRuleTriggerDTO.LoyaltyRuleTriggerId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LoyaltyRuleId", loyaltyRuleTriggerDTO.LoyaltyRuleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ApplicableProductId", loyaltyRuleTriggerDTO.ApplicableProductId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ApplicableProductTypeId", loyaltyRuleTriggerDTO.ApplicableProductTypeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ApplicableGameId", loyaltyRuleTriggerDTO.ApplicableGameId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ApplicableGameProfileId", loyaltyRuleTriggerDTO.ApplicableGameProfileId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ApplicablePOSTypeId", loyaltyRuleTriggerDTO.ApplicablePOSTypeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ExcludeFlag", loyaltyRuleTriggerDTO.ExcludeFlag));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", loyaltyRuleTriggerDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CategoryId", loyaltyRuleTriggerDTO.CategoryId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", loyaltyRuleTriggerDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }
        /// <summary>
        /// Converts the Data row object to LoyaltyRuleTriggerDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the object of LoyaltyRuleTriggerDTO</returns>
        private LoyaltyRuleTriggerDTO GetLoyaltyRuleTriggerDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            LoyaltyRuleTriggerDTO loyaltyRuleTriggerDTO = new LoyaltyRuleTriggerDTO(dataRow["LoyaltyRuleTriggerId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LoyaltyRuleTriggerId"]),
                                                dataRow["LoyaltyRuleId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LoyaltyRuleId"]),
                                                dataRow["ApplicableProductId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ApplicableProductId"]),
                                                dataRow["ApplicableProductTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ApplicableProductTypeId"]),
                                                dataRow["ApplicableGameId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ApplicableGameId"]),
                                                dataRow["ApplicableGameProfileId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ApplicableGameProfileId"]),
                                                dataRow["ApplicablePOSTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ApplicablePOSTypeId"]),
                                                dataRow["ExcludeFlag"] == DBNull.Value ? 'N' : Convert.ToChar(dataRow["ExcludeFlag"]),
                                                dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                                dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                dataRow["CategoryId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CategoryId"]),
                                                dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]));
            return loyaltyRuleTriggerDTO;
        }

        /// <summary>
        /// Gets the LoyaltyRuleTrigger data of passed LoyaltyRuleTrigger ID
        /// </summary>
        /// <param name="lrtgerId">lrtgerId is passed as parameter</param>
        /// <returns>Returns LoyaltyRuleTriggerDTO</returns>
        public LoyaltyRuleTriggerDTO GetLoyaltyRuleTriggerDTO(int loyaltyRuleTriggerId)
        {
            log.LogMethodEntry(loyaltyRuleTriggerId);
            LoyaltyRuleTriggerDTO result = null;
            string query = SELECT_QUERY + @" WHERE lrt.LoyaltyRuleTriggerId = @LoyaltyRuleTriggerId";
            SqlParameter parameter = new SqlParameter("@LoyaltyRuleTriggerId", loyaltyRuleTriggerId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetLoyaltyRuleTriggerDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        ///  Deletes the LoyaltyRuleTrigger record
        /// </summary>
        /// <param name="loyaltyRuleTriggerDTO">LoyaltyRuleTriggerDTO is passed as parameter</param>
        internal void Delete(LoyaltyRuleTriggerDTO loyaltyRuleTriggerDTO)
        {
            log.LogMethodEntry(loyaltyRuleTriggerDTO);
            string query = @"DELETE  
                             FROM LoyaltyRuleTriggers
                             WHERE LoyaltyRuleTriggers.LoyaltyRuleTriggerId = @LoyaltyRuleTriggerId";
            SqlParameter parameter = new SqlParameter("@LoyaltyRuleTriggerId", loyaltyRuleTriggerDTO.LoyaltyRuleTriggerId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            loyaltyRuleTriggerDTO.AcceptChanges();
            log.LogMethodExit();
        }
        
        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="loyaltyRuleTriggerDTO">LoyaltyRuleTriggerDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        private void RefreshLoyaltyRuleTriggerDTO(LoyaltyRuleTriggerDTO loyaltyRuleTriggerDTO, DataTable dt)
        {
            log.LogMethodEntry(loyaltyRuleTriggerDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                loyaltyRuleTriggerDTO.LoyaltyRuleTriggerId = Convert.ToInt32(dt.Rows[0]["LoyaltyRuleTriggerId"]);
                loyaltyRuleTriggerDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                loyaltyRuleTriggerDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                loyaltyRuleTriggerDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                loyaltyRuleTriggerDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                loyaltyRuleTriggerDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                loyaltyRuleTriggerDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///  Inserts the record to the LoyaltyRuleTrigger Table. 
        /// </summary>
        /// <param name="loyaltyRuleTriggerDTO">LoyaltyRuleTriggerDTO is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated LoyaltyRuleTriggerDTO</returns>
        public LoyaltyRuleTriggerDTO Insert(LoyaltyRuleTriggerDTO loyaltyRuleTriggerDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(loyaltyRuleTriggerDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[LoyaltyRuleTriggers]
                            (
                            LoyaltyRuleId,
                            ApplicableProductId,
                            ApplicableProductTypeId,
                            ApplicableGameId,
                            ApplicableGameProfileId,
                            ApplicablePOSTypeId,
                            ExcludeFlag,
                            LastUpdatedDate,
                            LastUpdatedBy,
                            Guid,
                            site_id,
                            MasterEntityId,
                            CategoryId,
                            CreatedBy,
                            CreationDate, IsActive
                            )
                            VALUES
                            (
                            @LoyaltyRuleId,
                            @ApplicableProductId,
                            @ApplicableProductTypeId,
                            @ApplicableGameId,
                            @ApplicableGameProfileId,
                            @ApplicablePOSTypeId,
                            @ExcludeFlag,
                            GETDATE(),
                            @LastUpdatedBy,
                            NEWID(),
                            @site_id,
                            @MasterEntityId,
                            @CategoryId,
                            @CreatedBy,
                            GETDATE(), @IsActive
                            )
                            SELECT * FROM LoyaltyRuleTriggers WHERE LoyaltyRuleTriggerId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(loyaltyRuleTriggerDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshLoyaltyRuleTriggerDTO(loyaltyRuleTriggerDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting LoyaltyRuleTriggerDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(loyaltyRuleTriggerDTO);
            return loyaltyRuleTriggerDTO;
        }

        /// <summary>
        /// Update the record in the LoyaltyRuleTriggers Table. 
        /// </summary>
        /// <param name="loyaltyRuleTriggerDTO">LoyaltyRuleTriggerDTO is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated LoyaltyRuleTriggerDTO</returns>
        public LoyaltyRuleTriggerDTO Update(LoyaltyRuleTriggerDTO loyaltyRuleTriggerDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(loyaltyRuleTriggerDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[LoyaltyRuleTriggers]
                             SET
                             LoyaltyRuleId = @LoyaltyRuleId,
                             ApplicableProductId = @ApplicableProductId,
                             ApplicableProductTypeId = @ApplicableProductTypeId,
                             ApplicableGameId = @ApplicableGameId,
                             ApplicableGameProfileId = @ApplicableGameProfileId,
                             ApplicablePOSTypeId = @ApplicablePOSTypeId,
                             ExcludeFlag = @ExcludeFlag,
                             LastUpdatedDate = GETDATE(),
                             LastUpdatedBy = @LastUpdatedBy,
                             MasterEntityId = @MasterEntityId,
                             CategoryId = @CategoryId,
                             IsActive = @IsActive
                             WHERE LoyaltyRuleTriggerId = @LoyaltyRuleTriggerId
                            SELECT * FROM LoyaltyRuleTriggers WHERE LoyaltyRuleTriggerId = @LoyaltyRuleTriggerId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(loyaltyRuleTriggerDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshLoyaltyRuleTriggerDTO(loyaltyRuleTriggerDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating LoyaltyRuleTriggerDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(loyaltyRuleTriggerDTO);
            return loyaltyRuleTriggerDTO;
        }

        /// <summary>
        /// Returns the List of LoyaltyRuleTriggerDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of LoyaltyRuleTriggerDTO</returns>
        public List<LoyaltyRuleTriggerDTO> GetLoyaltyRuleTriggerDTOList(List<KeyValuePair<LoyaltyRuleTriggerDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<LoyaltyRuleTriggerDTO> lrtgerDTOList = new List<LoyaltyRuleTriggerDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<LoyaltyRuleTriggerDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == LoyaltyRuleTriggerDTO.SearchByParameters.LOYALTY_RULE_TRIG_ID ||
                            searchParameter.Key == LoyaltyRuleTriggerDTO.SearchByParameters.LOYALTY_RULE_ID ||
                            searchParameter.Key == LoyaltyRuleTriggerDTO.SearchByParameters.APPLICABLE_GAME_ID ||
                            searchParameter.Key == LoyaltyRuleTriggerDTO.SearchByParameters.APPLICABLE_GAME_PROFILE_ID ||
                            searchParameter.Key == LoyaltyRuleTriggerDTO.SearchByParameters.APPLICABLE_PRODUCT_ID ||
                            searchParameter.Key == LoyaltyRuleTriggerDTO.SearchByParameters.APPLICABLE_PRODUCT_TYPE_ID ||
                            searchParameter.Key == LoyaltyRuleTriggerDTO.SearchByParameters.CATEGORY_ID ||
                            searchParameter.Key == LoyaltyRuleTriggerDTO.SearchByParameters.POS_TYPE_ID ||
                            searchParameter.Key == LoyaltyRuleTriggerDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == LoyaltyRuleTriggerDTO.SearchByParameters.EXCLUDE_FLAG)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToChar(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == LoyaltyRuleTriggerDTO.SearchByParameters.LOYALTY_RULE_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " in (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == LoyaltyRuleTriggerDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == LoyaltyRuleTriggerDTO.SearchByParameters.SITE_ID)
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
                    LoyaltyRuleTriggerDTO lrtgerDTO = GetLoyaltyRuleTriggerDTO(dataRow);
                    lrtgerDTOList.Add(lrtgerDTO);
                }
            }
            log.LogMethodExit(lrtgerDTOList);
            return lrtgerDTOList;
        }

    }
}
