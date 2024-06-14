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
    /// LoyaltyBonusPurchaseCriteria Data Handler - Handles insert, update and selection of LoyaltyBonusPurchaseCriteria objects
    /// </summary>
   public class LoyaltyBonusPurchaseCriteriaDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM LoyaltyBonusPurchaseCriteria as lbpc ";

        /// <summary>
        /// Dictionary for searching Parameters for the LoyaltyBonusPurchaseCriteria object.
        /// </summary>
        private static readonly Dictionary<LoyaltyBonusPurchaseCriteriaDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<LoyaltyBonusPurchaseCriteriaDTO.SearchByParameters, string>
        {
            { LoyaltyBonusPurchaseCriteriaDTO.SearchByParameters.ID,"lbpc.Id"},
            { LoyaltyBonusPurchaseCriteriaDTO.SearchByParameters.LOYALTY_BONUS_ID,"lbpc.LoyaltyBonusId"},
            { LoyaltyBonusPurchaseCriteriaDTO.SearchByParameters.LOYALTY_BONUS_ID_LIST,"lbpc.LoyaltyBonusId"},
            { LoyaltyBonusPurchaseCriteriaDTO.SearchByParameters.POS_TYPE_ID,"lbpc.POSTypeId"},
            { LoyaltyBonusPurchaseCriteriaDTO.SearchByParameters.PRODUCT_ID,"lbpc.ProductId"},
            { LoyaltyBonusPurchaseCriteriaDTO.SearchByParameters.SITE_ID,"lbpc.site_id"},
            { LoyaltyBonusPurchaseCriteriaDTO.SearchByParameters.ACTIVE_FLAG,"lbpc.IsActive"},
            { LoyaltyBonusPurchaseCriteriaDTO.SearchByParameters.MASTER_ENTITY_ID,"lbpc.MasterEntityId"}
        };

        /// <summary>
        /// Parameterized Constructor for LoyaltyBonusPurchaseCriteriaDataHandler.
        /// </summary>
        /// <param name="sqlTransaction"><SqlTransaction object</param>
        public LoyaltyBonusPurchaseCriteriaDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating LoyaltyBonusPurchaseCriteria Record.
        /// </summary>
        /// <param name="loyaltyBonusPurchaseCriteriaDTO">LoyaltyBonusPurchaseCriteriaDTO object passed as Parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(LoyaltyBonusPurchaseCriteriaDTO loyaltyBonusPurchaseCriteriaDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(loyaltyBonusPurchaseCriteriaDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", loyaltyBonusPurchaseCriteriaDTO.Id));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LoyaltyBonusId", loyaltyBonusPurchaseCriteriaDTO.LoyaltyBonusId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@POSTypeId", loyaltyBonusPurchaseCriteriaDTO.POSTypeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProductId", loyaltyBonusPurchaseCriteriaDTO.ProductId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", loyaltyBonusPurchaseCriteriaDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", loyaltyBonusPurchaseCriteriaDTO.ActiveFlag));
            log.LogMethodExit(parameters);
            return parameters;
        }
        /// <summary>
        /// Converts the Data row object to LoyaltyBonusPurchaseCriteriaDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the object of LoyaltyBonusPurchaseCriteriaDTO</returns>
        private LoyaltyBonusPurchaseCriteriaDTO GetLoyaltyBonusPurchaseCriteriaDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            LoyaltyBonusPurchaseCriteriaDTO loyaltyBonusRewardCriteriaDTO = new LoyaltyBonusPurchaseCriteriaDTO(dataRow["Id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Id"]),
                                                dataRow["LoyaltyBonusId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LoyaltyBonusId"]),
                                                dataRow["POSTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["POSTypeId"]),
                                                dataRow["ProductId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ProductId"]),
                                                dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                                dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]));
            return loyaltyBonusRewardCriteriaDTO;
        }

        /// <summary>
        /// Gets the LoyaltyBonusPurchaseCriteria data of passed LoyaltyBonusPurchaseCriteria ID
        /// </summary>
        /// <param name="loyaltyBonusPurchaseCriteriaId">loyaltyBonusPurchaseCriteriaId is passed as parameter</param>
        /// <returns>Returns LoyaltyBonusPurchaseCriteriaDTO</returns>
        public LoyaltyBonusPurchaseCriteriaDTO GetLoyaltyBonusPurchaseCriteriaDTO(int loyaltyBonusPurchaseCriteriaId)
        {
            log.LogMethodEntry(loyaltyBonusPurchaseCriteriaId);
            LoyaltyBonusPurchaseCriteriaDTO result = null;
            string query = SELECT_QUERY + @" WHERE lbpc.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", loyaltyBonusPurchaseCriteriaId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetLoyaltyBonusPurchaseCriteriaDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        ///  Deletes the LoyaltyBonusPurchaseCriteria record
        /// </summary>
        /// <param name="loyaltyBonusPurchaseCriteriaDTO">LoyaltyBonusPurchaseCriteriaDTO is passed as parameter</param>
        internal void Delete(LoyaltyBonusPurchaseCriteriaDTO loyaltyBonusPurchaseCriteriaDTO)
        {
            log.LogMethodEntry(loyaltyBonusPurchaseCriteriaDTO);
            string query = @"DELETE  
                             FROM LoyaltyBonusPurchaseCriteria
                             WHERE LoyaltyBonusPurchaseCriteria.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", loyaltyBonusPurchaseCriteriaDTO.Id);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            loyaltyBonusPurchaseCriteriaDTO.AcceptChanges();
            log.LogMethodExit();
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="loyaltyBonusPurchaseCriteriaDTO">LoyaltyBonusPurchaseCriteriaDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        private void RefreshLoyaltyBonusPurchaseCriteriaDTO(LoyaltyBonusPurchaseCriteriaDTO loyaltyBonusPurchaseCriteriaDTO, DataTable dt)
        {
            log.LogMethodEntry(loyaltyBonusPurchaseCriteriaDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                loyaltyBonusPurchaseCriteriaDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                loyaltyBonusPurchaseCriteriaDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                loyaltyBonusPurchaseCriteriaDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                loyaltyBonusPurchaseCriteriaDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                loyaltyBonusPurchaseCriteriaDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                loyaltyBonusPurchaseCriteriaDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                loyaltyBonusPurchaseCriteriaDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///  Inserts the record to the LoyaltyBonusPurchaseCriteria Table. 
        /// </summary>
        /// <param name="loyaltyBonusPurchaseCriteriaDTO">LoyaltyBonusPurchaseCriteriaDTO object passed as Parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated LoyaltyBonusPurchaseCriteriaDTO</returns>
        public LoyaltyBonusPurchaseCriteriaDTO Insert(LoyaltyBonusPurchaseCriteriaDTO loyaltyBonusPurchaseCriteriaDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(loyaltyBonusPurchaseCriteriaDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[LoyaltyBonusPurchaseCriteria]
                            (
                            LoyaltyBonusId,
                            POSTypeId,
                            ProductId,
                            LastupdatedDate,
                            LastUpdatedBy,
                            Guid,
                            site_id,
                            MasterEntityId,
                            CreatedBy,
                            CreationDate, IsActive
                            )
                            VALUES
                            (
                            @LoyaltyBonusId,
                            @POSTypeId,
                            @ProductId,
                            GETDATE(),
                            @LastUpdatedBy,
                            NEWID(),
                            @site_id,
                            @MasterEntityId,
                            @CreatedBy,
                            GETDATE(), @IsActive
                            )
                            SELECT * FROM LoyaltyBonusPurchaseCriteria WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(loyaltyBonusPurchaseCriteriaDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshLoyaltyBonusPurchaseCriteriaDTO(loyaltyBonusPurchaseCriteriaDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting LoyaltyBonusPurchaseCriteriaDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(loyaltyBonusPurchaseCriteriaDTO);
            return loyaltyBonusPurchaseCriteriaDTO;
        }

        /// <summary>
        /// Update the record in the LoyaltyBonusPurchaseCriteria Table. 
        /// </summary>
        /// <param name="loyaltyBonusPurchaseCriteriaDTO">LoyaltyBonusPurchaseCriteriaDTO object passed as Parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated LoyaltyBonusPurchaseCriteriaDTO</returns>
        public LoyaltyBonusPurchaseCriteriaDTO Update(LoyaltyBonusPurchaseCriteriaDTO loyaltyBonusPurchaseCriteriaDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(loyaltyBonusPurchaseCriteriaDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[LoyaltyBonusPurchaseCriteria]
                             SET
                             LoyaltyBonusId = @LoyaltyBonusId,
                             POSTypeId = @POSTypeId,
                             ProductId = @ProductId,
                             LastupdatedDate = GETDATE(),
                             LastUpdatedBy = @LastUpdatedBy,
                             MasterEntityId = @MasterEntityId,
                             IsActive = @IsActive
                             WHERE Id = @Id
                            SELECT * FROM LoyaltyBonusPurchaseCriteria WHERE Id = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(loyaltyBonusPurchaseCriteriaDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshLoyaltyBonusPurchaseCriteriaDTO(loyaltyBonusPurchaseCriteriaDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating LoyaltyBonusPurchaseCriteriaDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(loyaltyBonusPurchaseCriteriaDTO);
            return loyaltyBonusPurchaseCriteriaDTO;
        }

        /// <summary>
        /// Returns the List of LoyaltyBonusRewardCriteriaDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of LoyaltyBonusPurchaseCriteriaDTO</returns>
        public List<LoyaltyBonusPurchaseCriteriaDTO> GetLoyaltyBonusPurchaseCriteriaDTO(List<KeyValuePair<LoyaltyBonusPurchaseCriteriaDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<LoyaltyBonusPurchaseCriteriaDTO> loyaltyBonusPurchaseCriteriaDTOList = new List<LoyaltyBonusPurchaseCriteriaDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<LoyaltyBonusPurchaseCriteriaDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == LoyaltyBonusPurchaseCriteriaDTO.SearchByParameters.ID ||
                            searchParameter.Key == LoyaltyBonusPurchaseCriteriaDTO.SearchByParameters.LOYALTY_BONUS_ID ||
                            searchParameter.Key == LoyaltyBonusPurchaseCriteriaDTO.SearchByParameters.POS_TYPE_ID ||
                            searchParameter.Key == LoyaltyBonusPurchaseCriteriaDTO.SearchByParameters.PRODUCT_ID ||
                            searchParameter.Key == LoyaltyBonusPurchaseCriteriaDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == LoyaltyBonusPurchaseCriteriaDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == LoyaltyBonusPurchaseCriteriaDTO.SearchByParameters.LOYALTY_BONUS_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == LoyaltyBonusPurchaseCriteriaDTO.SearchByParameters.ACTIVE_FLAG)
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
                    LoyaltyBonusPurchaseCriteriaDTO loyaltyBonusPurchaseCriteriaDTO = GetLoyaltyBonusPurchaseCriteriaDTO(dataRow);
                    loyaltyBonusPurchaseCriteriaDTOList.Add(loyaltyBonusPurchaseCriteriaDTO);
                }
            }
            log.LogMethodExit(loyaltyBonusPurchaseCriteriaDTOList);
            return loyaltyBonusPurchaseCriteriaDTOList;
        }

    }
}
