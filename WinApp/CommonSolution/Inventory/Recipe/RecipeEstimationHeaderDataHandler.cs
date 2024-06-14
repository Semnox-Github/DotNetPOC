/********************************************************************************************
 * Project Name - Inventory
 * Description  - Data handler  object of RecipeEstimationHeader
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By         Remarks          
 *********************************************************************************************
 *2.100.0       19-Jul-2020   Deeksha             Created for Recipe Management enhancement.
 *********************************************************************************************/
using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Globalization;
using Semnox.Core.Utilities;
using System.Collections.Generic;

namespace Semnox.Parafait.Inventory.Recipe
{
    /// <summary>
    /// Recipe Estimation Header DataHandler
    /// </summary>
    public class RecipeEstimationHeaderDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM RecipeEstimationHeader AS rpEstHdr ";

        private static readonly Dictionary<RecipeEstimationHeaderDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<RecipeEstimationHeaderDTO.SearchByParameters, string>
            {
                {RecipeEstimationHeaderDTO.SearchByParameters.RECIPE_ESTIMATION_HEADER_ID, "rpEstHdr.RecipeEstimationHeaderId"},
                {RecipeEstimationHeaderDTO.SearchByParameters.IS_ACTIVE, "rpEstHdr.IsActive"},
                {RecipeEstimationHeaderDTO.SearchByParameters.CURRENT_FROM_DATE, "rpEstHdr.FromDate"},
                {RecipeEstimationHeaderDTO.SearchByParameters.FROM_DATE, "rpEstHdr.FromDate"},
                {RecipeEstimationHeaderDTO.SearchByParameters.DATE_NOT_IN_JOB_DATA, "rpEstHdr.FromDate"},
                {RecipeEstimationHeaderDTO.SearchByParameters.TO_DATE, "rpEstHdr.ToDate"},
                {RecipeEstimationHeaderDTO.SearchByParameters.CURRENT_TO_DATE, "rpEstHdr.ToDate"},
                {RecipeEstimationHeaderDTO.SearchByParameters.MASTER_ENTITY_ID, "rpEstHdr.MasterEntityId"},
                {RecipeEstimationHeaderDTO.SearchByParameters.SITE_ID, "rpEstHdr.site_id"},
                {RecipeEstimationHeaderDTO.SearchByParameters.RECIPE_ESTIMATION_HEADER_ID_LIST, "rpEstHdr.RecipeEstimationHeaderId"}
            };

        /// <summary>
        /// Parameterized Constructor for RecipeEstimationHeaderDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction Object</param>
        public RecipeEstimationHeaderDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        private List<SqlParameter> GetSQLParameters(RecipeEstimationHeaderDTO recipeEstimationHeaderDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(recipeEstimationHeaderDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@RecipeEstimationHeaderId", recipeEstimationHeaderDTO.RecipeEstimationHeaderId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@FromDate", recipeEstimationHeaderDTO.FromDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ToDate", recipeEstimationHeaderDTO.ToDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@AspirationalPercentage", recipeEstimationHeaderDTO.AspirationalPercentage));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SeasonalPercentage", recipeEstimationHeaderDTO.SeasonalPercentage));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ConsiderEventPromotions", recipeEstimationHeaderDTO.ConsiderEventPromotions));
            parameters.Add(dataAccessHandler.GetSQLParameter("@HistoricalDataInDays", recipeEstimationHeaderDTO.HistoricalDataInDays));
            parameters.Add(dataAccessHandler.GetSQLParameter("@EventOffsetHrs", recipeEstimationHeaderDTO.EventOffsetHrs));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IncludeFinishedItem", recipeEstimationHeaderDTO.IncludeFinishedItem));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IncludeSemiFinishedItem", recipeEstimationHeaderDTO.IncludeSemiFinishedItem));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", recipeEstimationHeaderDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", recipeEstimationHeaderDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastupdatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        private RecipeEstimationHeaderDTO GetRecipeEstimationHeaderDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            RecipeEstimationHeaderDTO recipeEstimationHeaderDTO = new RecipeEstimationHeaderDTO(
                dataRow["RecipeEstimationHeaderId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["RecipeEstimationHeaderId"]),
                dataRow["FromDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["FromDate"]),
                dataRow["ToDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["ToDate"]),
                dataRow["AspirationalPercentage"] == DBNull.Value ? (Decimal?)null : Convert.ToDecimal(dataRow["AspirationalPercentage"]),
                dataRow["SeasonalPercentage"] == DBNull.Value ? (Decimal?)null : Convert.ToDecimal(dataRow["SeasonalPercentage"]),
                dataRow["ConsiderEventPromotions"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["ConsiderEventPromotions"]),
                dataRow["HistoricalDataInDays"] == DBNull.Value ? 30 : Convert.ToInt32(dataRow["HistoricalDataInDays"]),
                dataRow["EventOffsetHrs"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["EventOffsetHrs"]),
                dataRow["IncludeFinishedItem"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["IncludeFinishedItem"]),
                dataRow["IncludeSemiFinishedItem"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["IncludeSemiFinishedItem"]),
                dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"])
                );
            log.LogMethodExit(recipeEstimationHeaderDTO);
            return recipeEstimationHeaderDTO;
        }

        internal RecipeEstimationHeaderDTO GetRecipeEstimationHeaderDTO(int recipeEstimationHeaderId)
        {
            log.LogMethodEntry(recipeEstimationHeaderId);
            RecipeEstimationHeaderDTO result = null;
            string query = SELECT_QUERY + @" WHERE rpEstHdr.RecipeEstimationHeaderId = @RecipeEstimationHeaderId";
            SqlParameter parameter = new SqlParameter("@RecipeEstimationHeaderId", recipeEstimationHeaderId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetRecipeEstimationHeaderDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        internal void Delete(int id)
        {
            log.LogMethodEntry(id);
            string query = @"DELETE  
                             FROM RecipeEstimationHeader
                             WHERE RecipeEstimationHeaderId = @RecipeEstimationHeaderId";
            SqlParameter parameter = new SqlParameter("@RecipeEstimationHeaderId", id);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Method to Purge fore-casted Data
        /// </summary>
        /// <param name="purgeBeforeDays"></param>
        internal void PurgeOldData(int purgeBeforeDays)
        {
            log.LogMethodEntry(purgeBeforeDays);
            string query = @"BEGIN
                                delete from RecipeEstimationHeader where 
                                FromDate <  dateadd(DAY, -@PurgeBeforeDays, getdate())
			                    and  datepart(HOUR,FromDate) != 00
                                update statistics RecipeEstimationHeader with sample 100 percent
                             END";
            SqlParameter parameter = new SqlParameter("@PurgeBeforeDays", purgeBeforeDays);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit();
        }

        private void RefreshRecipeEstimationHeaderDTO(RecipeEstimationHeaderDTO recipeEstimationHeaderDTO, DataTable dt)
        {
            log.LogMethodEntry(recipeEstimationHeaderDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                recipeEstimationHeaderDTO.RecipeEstimationHeaderId = Convert.ToInt32(dt.Rows[0]["RecipeEstimationHeaderId"]);
                recipeEstimationHeaderDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                recipeEstimationHeaderDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                recipeEstimationHeaderDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                recipeEstimationHeaderDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                recipeEstimationHeaderDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                recipeEstimationHeaderDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        internal RecipeEstimationHeaderDTO Insert(RecipeEstimationHeaderDTO recipeEstimationHeaderDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(recipeEstimationHeaderDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[RecipeEstimationHeader]
                               (  [FromDate],
                                  [ToDate],
                                  [AspirationalPercentage],
                                  [SeasonalPercentage],
                                  [ConsiderEventPromotions],
                                  [HistoricalDataInDays],
                                  [EventOffsetHrs],
                                  [IncludeFinishedItem],
                                  [IncludeSemiFinishedItem],
                                  [IsActive],
                                  [CreatedBy],
                                  [CreationDate],
                                  [LastUpdatedBy],
                                  [LastUpdateDate],
                                  [site_id],
                                  [Guid],
                                  [MasterEntityId])
                         VALUES
                               (    @FromDate,
                                    @ToDate,
                                    @AspirationalPercentage,
                                    @SeasonalPercentage,
                                    @ConsiderEventPromotions,
                                    @HistoricalDataInDays,
                                    @EventOffsetHrs,
                                    @IncludeFinishedItem,
                                    @IncludeSemiFinishedItem,
                                    @IsActive,
                                    @CreatedBy,
                                    GETDATE(),
                                    @LastUpdatedBy,
                                    GETDATE(),
                                    @SiteId,
                                    NEWID(), 
                                    @MasterEntityId )
                                SELECT * FROM RecipeEstimationHeader WHERE RecipeEstimationHeaderId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(recipeEstimationHeaderDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshRecipeEstimationHeaderDTO(recipeEstimationHeaderDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(recipeEstimationHeaderDTO);
            return recipeEstimationHeaderDTO;
        }


        internal RecipeEstimationHeaderDTO Update(RecipeEstimationHeaderDTO recipeEstimationHeaderDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(recipeEstimationHeaderDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[RecipeEstimationHeader] set
                               [FromDate]               = @FromDate,
                               [ToDate]                 = @ToDate,
                               [AspirationalPercentage] = @AspirationalPercentage,
                               [SeasonalPercentage]     = @SeasonalPercentage,
                               [ConsiderEventPromotions]= @ConsiderEventPromotions,
                               [HistoricalDataInDays]   = @HistoricalDataInDays,
                               [EventOffsetHrs]         = @EventOffsetHrs,
                               [IncludeFinishedItem]    = @IncludeFinishedItem,
                               [IncludeSemiFinishedItem]= @IncludeSemiFinishedItem,
                               [MasterEntityId]         = @MasterEntityId,
                               [IsActive]               = @IsActive,
                               [LastUpdatedBy]          = @LastUpdatedBy,
                               [LastUpdateDate]         = GETDATE()
                               where RecipeEstimationHeaderId = @RecipeEstimationHeaderId
                             SELECT * FROM RecipeEstimationHeader WHERE RecipeEstimationHeaderId = @RecipeEstimationHeaderId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(recipeEstimationHeaderDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshRecipeEstimationHeaderDTO(recipeEstimationHeaderDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(recipeEstimationHeaderDTO);
            return recipeEstimationHeaderDTO;
        }

        

        internal List<RecipeEstimationHeaderDTO> GetRecipeEstimationHeaderDTOList(List<KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<RecipeEstimationHeaderDTO> recipeEstimationHeaderDTOList = new List<RecipeEstimationHeaderDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Any()))
            {
                string joiner = string.Empty; 
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == RecipeEstimationHeaderDTO.SearchByParameters.RECIPE_ESTIMATION_HEADER_ID ||
                            searchParameter.Key == RecipeEstimationHeaderDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == RecipeEstimationHeaderDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == RecipeEstimationHeaderDTO.SearchByParameters.CURRENT_FROM_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == RecipeEstimationHeaderDTO.SearchByParameters.CURRENT_TO_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) <= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == RecipeEstimationHeaderDTO.SearchByParameters.FROM_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == RecipeEstimationHeaderDTO.SearchByParameters.DATE_NOT_IN_JOB_DATA)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) != " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == RecipeEstimationHeaderDTO.SearchByParameters.TO_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) <= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == RecipeEstimationHeaderDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == RecipeEstimationHeaderDTO.SearchByParameters.RECIPE_ESTIMATION_HEADER_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " in (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
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
            if (dataTable != null && dataTable.Rows.Cast<DataRow>().Any())
            {
                recipeEstimationHeaderDTOList = dataTable.Rows.Cast<DataRow>().Select(x => GetRecipeEstimationHeaderDTO(x)).ToList();
            }
            log.LogMethodExit(recipeEstimationHeaderDTOList);
            return recipeEstimationHeaderDTOList;
        }
    }
}
