/********************************************************************************************
 * Project Name - Promotions
 * Description  - Data Handler File for LoyaltyBonusRewardCriteria
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks 
 *********************************************************************************************
 *2.80     10-June-2019   Divya A                 Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Promotions
{
    /// <summary>
    /// LoyaltyBonusRewardCriteria Data Handler - Handles insert, update and selection of LoyaltyBonusRewardCriteria objects
    /// </summary>
   public  class LoyaltyBonusRewardCriteriaDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM LoyaltyBonusRewardCriteria as lbrc ";

        /// <summary>
        /// Dictionary for searching Parameters for the LoyaltyBonusRewardCriteria object.
        /// </summary>
        private static readonly Dictionary<LoyaltyBonusRewardCriteriaDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<LoyaltyBonusRewardCriteriaDTO.SearchByParameters, string>
        {
            { LoyaltyBonusRewardCriteriaDTO.SearchByParameters.ID,"lbrc.Id"},
            { LoyaltyBonusRewardCriteriaDTO.SearchByParameters.LOYALTY_BONUS_ID,"lbrc.LoyaltyBonusId"},
            { LoyaltyBonusRewardCriteriaDTO.SearchByParameters.LOYALTY_BONUS_ID_LIST,"lbrc.LoyaltyBonusId"},
            { LoyaltyBonusRewardCriteriaDTO.SearchByParameters.POS_TYPE_ID,"lbrc.POSTypeId"},
            { LoyaltyBonusRewardCriteriaDTO.SearchByParameters.PRODUCT_ID,"lbrc.ProductId"},
            { LoyaltyBonusRewardCriteriaDTO.SearchByParameters.GAME_ID,"lbrc.GameId"},
            { LoyaltyBonusRewardCriteriaDTO.SearchByParameters.GAME_PROFILE_ID,"lbrcGameProfileId"},
            { LoyaltyBonusRewardCriteriaDTO.SearchByParameters.CATEGORY_ID,"lbrcCategoryId"},
            { LoyaltyBonusRewardCriteriaDTO.SearchByParameters.ACTIVE_FLAG,"lbrc.IsActive"},
            { LoyaltyBonusRewardCriteriaDTO.SearchByParameters.SITE_ID,"lbrc.site_id"},
            { LoyaltyBonusRewardCriteriaDTO.SearchByParameters.MASTER_ENTITY_ID,"lbrc.MasterEntityId"}
        };

        /// <summary>
        /// Parameterized Constructor for LoyaltyBonusRewardCriteriaDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction Object</param>
        public LoyaltyBonusRewardCriteriaDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating LoyaltyBonusRewardCriteria Record.
        /// </summary>
        /// <param name="loyaltyBonusRewardCriteriaDTO">LoyaltyBonusRewardCriteriaDTO object is passed as parameter.</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(LoyaltyBonusRewardCriteriaDTO loyaltyBonusRewardCriteriaDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(loyaltyBonusRewardCriteriaDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", loyaltyBonusRewardCriteriaDTO.Id));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LoyaltyBonusId", loyaltyBonusRewardCriteriaDTO.LoyaltyBonusId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@POSTypeId", loyaltyBonusRewardCriteriaDTO.POSTypeId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProductId", loyaltyBonusRewardCriteriaDTO.ProductId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@GameProfileId", loyaltyBonusRewardCriteriaDTO.GameProfileId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@GameId", loyaltyBonusRewardCriteriaDTO.GameId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DiscountPercentage", loyaltyBonusRewardCriteriaDTO.DiscountPercentage));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DiscountedPrice", loyaltyBonusRewardCriteriaDTO.DiscountedPrice));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CategoryId", loyaltyBonusRewardCriteriaDTO.CategoryId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DiscountAmount", loyaltyBonusRewardCriteriaDTO.DiscountAmount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", loyaltyBonusRewardCriteriaDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", loyaltyBonusRewardCriteriaDTO.ActiveFlag));
            log.LogMethodExit(parameters);
            return parameters;
        }
        /// <summary>
        /// Converts the Data row object to LoyaltyBonusRewardCriteriaDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the object of LoyaltyBonusRewardCriteriaDTO</returns>
        private LoyaltyBonusRewardCriteriaDTO GetLoyaltyBonusRewardCriteriaDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            LoyaltyBonusRewardCriteriaDTO loyaltyBonusRewardCriteriaDTO = new LoyaltyBonusRewardCriteriaDTO(dataRow["Id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Id"]),
                                                dataRow["LoyaltyBonusId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LoyaltyBonusId"]),
                                                dataRow["POSTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["POSTypeId"]),
                                                dataRow["ProductId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ProductId"]),
                                                dataRow["GameProfileId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["GameProfileId"]),
                                                dataRow["GameId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["GameId"]),
                                                dataRow["DiscountPercentage"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["DiscountPercentage"]),
                                                dataRow["DiscountedPrice"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["DiscountedPrice"]),
                                                dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                                dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                dataRow["CategoryId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CategoryId"]),
                                                dataRow["DiscountAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["DiscountAmount"]),
                                                dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                dataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["IsActive"]));
            return loyaltyBonusRewardCriteriaDTO;
        }

        /// <summary>
        /// Gets the LoyaltyBonusRewardCriteria data of passed LoyaltyBonusRewardCriteria ID
        /// </summary>
        /// <param name="loyaltyBonusRewardCriteriaId">loyaltyBonusRewardCriteriaId is passed as parameter</param>
        /// <returns>Returns LoyaltyBonusRewardCriteriaDTO</returns>
        public LoyaltyBonusRewardCriteriaDTO GetLoyaltyBonusRewardCriteriaDTO(int loyaltyBonusRewardCriteriaId)
        {
            log.LogMethodEntry(loyaltyBonusRewardCriteriaId);
            LoyaltyBonusRewardCriteriaDTO result = null;
            string query = SELECT_QUERY + @" WHERE lbrc.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", loyaltyBonusRewardCriteriaId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetLoyaltyBonusRewardCriteriaDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        ///  Deletes the LoyaltyBonusRewardCriteria record
        /// </summary>
        /// <param name="loyaltyBonusRewardCriteriaDTO">LoyaltyBonusRewardCriteriaDTO is passed as parameter</param>
        internal void Delete(LoyaltyBonusRewardCriteriaDTO loyaltyBonusRewardCriteriaDTO)
        {
            log.LogMethodEntry(loyaltyBonusRewardCriteriaDTO);
            string query = @"DELETE  
                             FROM LoyaltyBonusRewardCriteria
                             WHERE LoyaltyBonusRewardCriteria.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", loyaltyBonusRewardCriteriaDTO.Id);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            loyaltyBonusRewardCriteriaDTO.AcceptChanges();
            log.LogMethodExit();
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="loyaltyBonusRewardCriteriaDTO">LoyaltyBonusRewardCriteriaDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        private void RefreshLoyaltyBonusRewardCriteriaDTO(LoyaltyBonusRewardCriteriaDTO loyaltyBonusRewardCriteriaDTO, DataTable dt)
        {
            log.LogMethodEntry(loyaltyBonusRewardCriteriaDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                loyaltyBonusRewardCriteriaDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                loyaltyBonusRewardCriteriaDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                loyaltyBonusRewardCriteriaDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                loyaltyBonusRewardCriteriaDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                loyaltyBonusRewardCriteriaDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                loyaltyBonusRewardCriteriaDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                loyaltyBonusRewardCriteriaDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///  Inserts the record to the LoyaltyBonusRewardCriteria Table. 
        /// </summary>
        /// <param name="loyaltyBonusRewardCriteriaDTO">LoyaltyBonusRewardCriteriaDTO object is passed as parameter.</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated LoyaltyBonusRewardCriteriaDTO</returns>
        public LoyaltyBonusRewardCriteriaDTO Insert(LoyaltyBonusRewardCriteriaDTO loyaltyBonusRewardCriteriaDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(loyaltyBonusRewardCriteriaDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[LoyaltyBonusRewardCriteria]
                            (
                            LoyaltyBonusId,
                            POSTypeId,
                            ProductId,
                            GameProfileId,
                            GameId,
                            DiscountPercentage,
                            DiscountedPrice,
                            LastupdatedDate,
                            LastUpdatedBy,
                            Guid,
                            site_id,
                            MasterEntityId,
                            CategoryId,
                            DiscountAmount,
                            CreatedBy,
                            CreationDate,IsActive
                            )
                            VALUES
                            (
                            @LoyaltyBonusId,
                            @POSTypeId,
                            @ProductId,
                            @GameProfileId,
                            @GameId,
                            @DiscountPercentage,
                            @DiscountedPrice,
                            GETDATE(),
                            @LastUpdatedBy,
                            NEWID(),
                            @site_id,
                            @MasterEntityId,
                            @CategoryId,
                            @DiscountAmount,
                            @CreatedBy,
                            GETDATE(), @IsActive
                            )
                            SELECT * FROM LoyaltyBonusRewardCriteria WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(loyaltyBonusRewardCriteriaDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshLoyaltyBonusRewardCriteriaDTO(loyaltyBonusRewardCriteriaDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting LoyaltyBonusRewardCriteriaDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(loyaltyBonusRewardCriteriaDTO);
            return loyaltyBonusRewardCriteriaDTO;
        }

        /// <summary>
        /// Update the record in the LoyaltyBonusRewardCriteria Table. 
        /// </summary>
        /// <param name="loyaltyBonusRewardCriteriaDTO">LoyaltyBonusRewardCriteriaDTO object is passed as parameter.</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated LoyaltyBonusRewardCriteriaDTO</returns>
        public LoyaltyBonusRewardCriteriaDTO Update(LoyaltyBonusRewardCriteriaDTO loyaltyBonusRewardCriteriaDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(loyaltyBonusRewardCriteriaDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[LoyaltyBonusRewardCriteria]
                             SET
                             LoyaltyBonusId = @LoyaltyBonusId,
                             POSTypeId = @POSTypeId,
                             ProductId = @ProductId,
                             GameProfileId = @GameProfileId,
                             GameId = @GameId,
                             DiscountPercentage = @DiscountPercentage,
                             DiscountedPrice = @DiscountedPrice,
                             LastupdatedDate = GETDATE(),
                             LastUpdatedBy = @LastUpdatedBy,
                             MasterEntityId = @MasterEntityId,
                             CategoryId = @CategoryId,
                             DiscountAmount = @DiscountAmount,
                             IsActive = @IsActive
                             WHERE Id = @Id
                            SELECT * FROM LoyaltyBonusRewardCriteria WHERE Id = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(loyaltyBonusRewardCriteriaDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshLoyaltyBonusRewardCriteriaDTO(loyaltyBonusRewardCriteriaDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating LoyaltyBonusRewardCriteriaDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(loyaltyBonusRewardCriteriaDTO);
            return loyaltyBonusRewardCriteriaDTO;
        }

        /// <summary>
        /// Returns the List of LoyaltyBonusRewardCriteriaDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of LoyaltyBonusRewardCriteriaDTO</returns>
        public List<LoyaltyBonusRewardCriteriaDTO> GetLoyaltyBonusRewardCriteriaDTO(List<KeyValuePair<LoyaltyBonusRewardCriteriaDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<LoyaltyBonusRewardCriteriaDTO> loyaltyBonusRewardCriteriaDTOList = new List<LoyaltyBonusRewardCriteriaDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<LoyaltyBonusRewardCriteriaDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == LoyaltyBonusRewardCriteriaDTO.SearchByParameters.ID ||
                            searchParameter.Key == LoyaltyBonusRewardCriteriaDTO.SearchByParameters.LOYALTY_BONUS_ID ||
                            searchParameter.Key == LoyaltyBonusRewardCriteriaDTO.SearchByParameters.GAME_ID ||
                            searchParameter.Key == LoyaltyBonusRewardCriteriaDTO.SearchByParameters.GAME_PROFILE_ID ||
                            searchParameter.Key == LoyaltyBonusRewardCriteriaDTO.SearchByParameters.POS_TYPE_ID ||
                            searchParameter.Key == LoyaltyBonusRewardCriteriaDTO.SearchByParameters.PRODUCT_ID ||
                            searchParameter.Key == LoyaltyBonusRewardCriteriaDTO.SearchByParameters.CATEGORY_ID ||
                            searchParameter.Key == LoyaltyBonusRewardCriteriaDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == LoyaltyBonusRewardCriteriaDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == LoyaltyBonusRewardCriteriaDTO.SearchByParameters.LOYALTY_BONUS_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == LoyaltyBonusRewardCriteriaDTO.SearchByParameters.ACTIVE_FLAG)
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
                    LoyaltyBonusRewardCriteriaDTO loyaltyBonusRewardCriteriaDTO = GetLoyaltyBonusRewardCriteriaDTO(dataRow);
                    loyaltyBonusRewardCriteriaDTOList.Add(loyaltyBonusRewardCriteriaDTO);
                }
            }
            log.LogMethodExit(loyaltyBonusRewardCriteriaDTOList);
            return loyaltyBonusRewardCriteriaDTOList;
        }

    }
}
