/********************************************************************************************
 * Project Name - Inventory
 * Description  - Data Handler object of RecipeEstimationDetails
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By         Remarks          
 *********************************************************************************************
 *2.100.0        23-Jul-2020   Deeksha             Created for Recipe Management enhancement.
 *********************************************************************************************/

using System;
using System.Data;
using System.Text;
using System.Linq;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using System.Collections.Generic;

namespace Semnox.Parafait.Inventory.Recipe
{
    /// <summary>
    /// Recipe Estimation Details DataHandler
    /// </summary>
    public class RecipeEstimationDetailsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT *,null EventDate FROM RecipeEstimationDetails AS rpdet ";

        private static readonly Dictionary<RecipeEstimationDetailsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<RecipeEstimationDetailsDTO.SearchByParameters, string>
            {
                {RecipeEstimationDetailsDTO.SearchByParameters.RECIPE_ESTIMATION_HEADER_ID, "rpdet.RecipeEstimationHeaderId"},
                {RecipeEstimationDetailsDTO.SearchByParameters.RECIPE_ESTIMATION_DETAIL_ID, "rpdet.RecipeEstimationDetailId"},
                {RecipeEstimationDetailsDTO.SearchByParameters.ACCOUNTING_CALENDAR_MASTER_ID, "rpdet.AccountingCalendarMasterId"},
                {RecipeEstimationDetailsDTO.SearchByParameters.IS_ACTIVE, "rpdet.IsActive"},
                {RecipeEstimationDetailsDTO.SearchByParameters.PRODUCT_ID, "rpdet.ProductId"},
                {RecipeEstimationDetailsDTO.SearchByParameters.UOM_ID, "rpdet.UOMId"},
                {RecipeEstimationDetailsDTO.SearchByParameters.MASTER_ENTITY_ID, "rpdet.MasterEntityId"},
                {RecipeEstimationDetailsDTO.SearchByParameters.SITE_ID, "rpdet.site_id"}
            };

        /// <summary>
        /// Parameterized Constructor for RecipeEstimationDetailsDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction Object</param>
        public RecipeEstimationDetailsDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        private List<SqlParameter> GetSQLParameters(RecipeEstimationDetailsDTO recipeEstimationDetailsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(recipeEstimationDetailsDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@RecipeEstimationDetailId", recipeEstimationDetailsDTO.RecipeEstimationDetailId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RecipeEstimationHeaderId", recipeEstimationDetailsDTO.RecipeEstimationHeaderId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProductId", recipeEstimationDetailsDTO.ProductId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@EstimatedQty", recipeEstimationDetailsDTO.EstimatedQty));
            parameters.Add(dataAccessHandler.GetSQLParameter("@EventQty", recipeEstimationDetailsDTO.EventQty));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TotalEstimatedQty", recipeEstimationDetailsDTO.TotalEstimatedQty));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PlannedQty", recipeEstimationDetailsDTO.PlannedQty));
            parameters.Add(dataAccessHandler.GetSQLParameter("@StockQty", recipeEstimationDetailsDTO.StockQty));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UOMId", recipeEstimationDetailsDTO.UOMId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@AccountingCalendarMasterId", recipeEstimationDetailsDTO.AccountingCalendarMasterId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", recipeEstimationDetailsDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", recipeEstimationDetailsDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }


        private RecipeEstimationDetailsDTO GetRecipeEstimationDetailDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            RecipeEstimationDetailsDTO recipeEstimationDetailsDTO = new RecipeEstimationDetailsDTO(dataRow["RecipeEstimationDetailId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["RecipeEstimationDetailId"]),
                                                dataRow["RecipeEstimationHeaderId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["RecipeEstimationHeaderId"]), 
                                                dataRow["ProductId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ProductId"]), 
                                                dataRow["EstimatedQty"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["EstimatedQty"]),
                                                dataRow["EventQty"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["EventQty"]),
                                                dataRow["TotalEstimatedQty"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["TotalEstimatedQty"]),
                                                dataRow["PlannedQty"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["PlannedQty"]),
                                                dataRow["StockQty"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["StockQty"]),
                                                dataRow["UOMId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["UOMId"]),
                                                dataRow["AccountingCalendarMasterId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["AccountingCalendarMasterId"]),
                                                dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                                                dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                                dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                dataRow["EventDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["EventDate"]));
            log.LogMethodExit(recipeEstimationDetailsDTO);
            return recipeEstimationDetailsDTO;
        }

        internal RecipeEstimationDetailsDTO GetRecipeEstimationDetailsId(int recipeEstimationDetailId)
        {
            log.LogMethodEntry(recipeEstimationDetailId);
            RecipeEstimationDetailsDTO result = null;
            string query = SELECT_QUERY + @" WHERE rpdet.RecipeEstimationDetailId = @RecipeEstimationDetailId";
            SqlParameter parameter = new SqlParameter("@RecipeEstimationDetailId", recipeEstimationDetailId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetRecipeEstimationDetailDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }


        internal void Delete(RecipeEstimationDetailsDTO recipeEstimationDetailsDTO)
        {
            log.LogMethodEntry(recipeEstimationDetailsDTO);
            string query = @"DELETE  
                             FROM RecipeEstimationDetails
                             WHERE RecipeEstimationDetails.RecipeEstimationDetailId = @RecipeEstimationDetailId";
            SqlParameter parameter = new SqlParameter("@RecipeEstimationDetailId", recipeEstimationDetailsDTO.RecipeEstimationDetailId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            recipeEstimationDetailsDTO.AcceptChanges();
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
                                delete from RecipeEstimationDetails where RecipeEstimationHeaderId in 
                                ( select RecipeEstimationHeaderId from RecipeEstimationHeader where
                                FromDate <  dateadd(DAY, -@purgeBeforeDays, getdate())
                                and  datepart(HOUR,FromDate) != 00)
                                update statistics RecipeEstimationDetails with sample 100 percent
                             END";
            SqlParameter parameter = new SqlParameter("@purgeBeforeDays", purgeBeforeDays);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit();
        }

        internal List<RecipeEstimationDetailsDTO> GetEventDetails(DateTime fromDate, DateTime todate)
        {
            log.LogMethodEntry(fromDate, todate);
            List<RecipeEstimationDetailsDTO> recipeEstimationDetailsDTOList = new List<RecipeEstimationDetailsDTO>();
            string query = @"select pb.ChildProductId ProductId,th.TrxDate EventDate ,sum(tl.quantity) * pb.Quantity as EventQty,--  sum(tl.quantity) EventQty,
                                null RecipeEstimationDetailId ,
                                null RecipeEstimationHeaderId, null EstimatedQty , null TotalEstimatedQty , null PlannedQty , null StockQty , 
                                null UOMId, null AccountingCalendarMasterId, null IsActive , null CreatedBy , null CreationDate , null LastUpdatedBy , 
                                null LastUpdateDate , null Guid , null site_id , null SynchStatus , null MasterEntityId
                                from Bookings b , trx_header th , trx_lines tl, product p , ProductBOM pb
				                where th.TrxId = b.TrxId and th.Status = 'RESERVED' and  b.Status != 'CANCELLED'   
				                and b.FromDate >= @fromDate and  b.FromDate < @todate and tl.TrxId = th.TrxId 
				                and tl.product_id = p.ManualProductId and p.ProductId = pb.ProductId group by pb.ChildProductId , th.TrxDate ,pb.Quantity";

            List<SqlParameter> parameter = new List<SqlParameter>();
            parameter.Add(new SqlParameter("@fromDate", fromDate));
            parameter.Add(new SqlParameter("@todate", todate));
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, parameter.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                recipeEstimationDetailsDTOList = dataTable.Rows.Cast<DataRow>().Select(x => GetRecipeEstimationDetailDTO(x)).ToList();
            }
            log.LogMethodExit(recipeEstimationDetailsDTOList);
            return recipeEstimationDetailsDTOList;
        }

        internal List<RecipeEstimationDetailsDTO> GetRecipeEstimationHeaderDTOListOfRecipe(List<int> recipeEstimationHeaderIdList,
                                                                                           bool activeRecords)
        {
            log.LogMethodEntry(recipeEstimationHeaderIdList, activeRecords);
            List<RecipeEstimationDetailsDTO> recipeEstimationDetailsList = new List<RecipeEstimationDetailsDTO>();
            string query = SELECT_QUERY + @" , @RecipeEstimationHeaderIdList List
                            WHERE RecipeEstimationHeaderId = List.Id ";
            if (activeRecords)
            {
                query += " AND isActive = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@RecipeEstimationHeaderIdList", recipeEstimationHeaderIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                recipeEstimationDetailsList = table.Rows.Cast<DataRow>().Select(x => GetRecipeEstimationDetailDTO(x)).ToList();
            }
            log.LogMethodExit(recipeEstimationDetailsList);
            return recipeEstimationDetailsList;
        }

        private void RefreshRecipeEstimationDetailsDTO(RecipeEstimationDetailsDTO recipeEstimationDetailsDTO, DataTable dt)
        {
            log.LogMethodEntry(recipeEstimationDetailsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                recipeEstimationDetailsDTO.RecipeEstimationDetailId = Convert.ToInt32(dt.Rows[0]["RecipeEstimationDetailId"]);
                recipeEstimationDetailsDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                recipeEstimationDetailsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                recipeEstimationDetailsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                recipeEstimationDetailsDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                recipeEstimationDetailsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                recipeEstimationDetailsDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        internal RecipeEstimationDetailsDTO Insert(RecipeEstimationDetailsDTO recipeEstimationDetailsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(recipeEstimationDetailsDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[RecipeEstimationDetails]
                               ([RecipeEstimationHeaderId]
                               ,[ProductId]
                               ,[EstimatedQty]
                               ,[EventQty]
                               ,[TotalEstimatedQty]
                               ,[PlannedQty]
                               ,[StockQty]
                               ,[UOMId]
                               ,[AccountingCalendarMasterId]
                               ,[IsActive]
                               ,[CreatedBy]
                               ,[CreationDate]
                               ,[LastUpdatedBy]
                               ,[LastUpdateDate]
                               ,[site_id]
                               ,[Guid]
                               ,[MasterEntityId] )
                               
                         VALUES
                               (
                                @RecipeEstimationHeaderId,
                                @ProductId,
                                @EstimatedQty,
                                @EventQty,
                                @TotalEstimatedQty,
                                @PlannedQty,
                                @StockQty,
                                @UOMId,
                                @AccountingCalendarMasterId,
                                @IsActive,
                                @CreatedBy,
                                GETDATE(),
                                @LastUpdatedBy,
                                GETDATE(),
                                @SiteId,
                                NEWID(), 
                                @MasterEntityId
                                 )
                                SELECT * FROM RecipeEstimationDetails WHERE RecipeEstimationDetailId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(recipeEstimationDetailsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshRecipeEstimationDetailsDTO(recipeEstimationDetailsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(recipeEstimationDetailsDTO);
            return recipeEstimationDetailsDTO;
        }


        internal RecipeEstimationDetailsDTO Update(RecipeEstimationDetailsDTO recipeEstimationDetailsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(recipeEstimationDetailsDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[RecipeEstimationDetails] set
                               [RecipeEstimationHeaderId]   = @RecipeEstimationHeaderId,
                               [ProductId]                  = @ProductId,
                               [EstimatedQty]               = @EstimatedQty,
                               [EventQty]                   = @EventQty,
                               [TotalEstimatedQty]          = @TotalEstimatedQty,
                               [PlannedQty]                 = @PlannedQty,
                               [StockQty]                   = @StockQty,
                               [UOMId]                      = @UOMId,
                               [IsActive]                   = @IsActive,
                               [MasterEntityId]             = @MasterEntityId,
                               [LastUpdatedBy]              = @LastUpdatedBy,
                               [LastUpdateDate]             = GETDATE()
                               where RecipeEstimationDetailId = @RecipeEstimationDetailId
                             SELECT * FROM RecipeEstimationDetails WHERE RecipeEstimationDetailId = @RecipeEstimationDetailId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(recipeEstimationDetailsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshRecipeEstimationDetailsDTO(recipeEstimationDetailsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(recipeEstimationDetailsDTO);
            return recipeEstimationDetailsDTO;
        }

        internal List<RecipeEstimationDetailsDTO> GetRecipeEstimationHeaderDTOList(List<KeyValuePair<RecipeEstimationDetailsDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<RecipeEstimationDetailsDTO> recipeEstimationDetailsDTOList = new List<RecipeEstimationDetailsDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<RecipeEstimationDetailsDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == RecipeEstimationDetailsDTO.SearchByParameters.RECIPE_ESTIMATION_DETAIL_ID ||
                            searchParameter.Key == RecipeEstimationDetailsDTO.SearchByParameters.RECIPE_ESTIMATION_HEADER_ID ||
                            searchParameter.Key == RecipeEstimationDetailsDTO.SearchByParameters.PRODUCT_ID ||
                            searchParameter.Key == RecipeEstimationDetailsDTO.SearchByParameters.UOM_ID ||
                            searchParameter.Key == RecipeEstimationDetailsDTO.SearchByParameters.ACCOUNTING_CALENDAR_MASTER_ID ||
                            searchParameter.Key == RecipeEstimationDetailsDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == RecipeEstimationDetailsDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == RecipeEstimationDetailsDTO.SearchByParameters.IS_ACTIVE)
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
                recipeEstimationDetailsDTOList = dataTable.Rows.Cast<DataRow>().Select(x => GetRecipeEstimationDetailDTO(x)).ToList();
            }
            log.LogMethodExit(recipeEstimationDetailsDTOList);
            return recipeEstimationDetailsDTOList;
        }
    }
}
