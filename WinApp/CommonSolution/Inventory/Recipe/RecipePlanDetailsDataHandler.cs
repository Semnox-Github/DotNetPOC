/********************************************************************************************
 * Project Name - Inventory
 * Description  - Data handler  object of RecipePlanDetails
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By         Remarks          
 *********************************************************************************************
 *2.100.0        22-Jul-2020   Deeksha             Created for Recipe Management enhancement.
 *********************************************************************************************/
using System;
using System.Linq;
using System.Text;
using System.Data;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Parafait.Inventory.Recipe
{
    /// <summary>
    /// Recipe Plan Details DataHandler
    /// </summary>
    public class RecipePlanDetailsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM RecipePlanDetails AS rpd ";

        private static readonly Dictionary<RecipePlanDetailsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<RecipePlanDetailsDTO.SearchByParameters, string>
            {
                {RecipePlanDetailsDTO.SearchByParameters.RECIPE_PLAN_DETAIL_ID, "rpd.RecipePlanDetailId"},
                {RecipePlanDetailsDTO.SearchByParameters.IS_ACTIVE, "rpd.IsActive"},
                {RecipePlanDetailsDTO.SearchByParameters.RECIPE_PLAN_HEADER_ID, "rpd.RecipePlanHeaderId"},
                {RecipePlanDetailsDTO.SearchByParameters.RECIPE_ESTIMATION_DETAIL_ID, "rpd.RecipeEstimationDetailId"},
                {RecipePlanDetailsDTO.SearchByParameters.PRODUCT_ID, "rpd.ProductId"},
                {RecipePlanDetailsDTO.SearchByParameters.UOM_ID, "rpd.UOMId"},
                {RecipePlanDetailsDTO.SearchByParameters.MASTER_ENTITY_ID, "rpd.MasterEntityId"},
                {RecipePlanDetailsDTO.SearchByParameters.SITE_ID, "rpd.site_id"},
            };

        /// <summary>
        /// Parameterized Constructor for RecipePlanDetailsDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction Object</param>
        public RecipePlanDetailsDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        private List<SqlParameter> GetSQLParameters(RecipePlanDetailsDTO recipePlanDetailsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(recipePlanDetailsDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@RecipePlanDetailId", recipePlanDetailsDTO.RecipePlanDetailId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RecipePlanHeaderId", recipePlanDetailsDTO.RecipePlanHeaderId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProductId", recipePlanDetailsDTO.ProductId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PlannedQty", recipePlanDetailsDTO.PlannedQty));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IncrementalQty", recipePlanDetailsDTO.IncrementalQty));
            parameters.Add(dataAccessHandler.GetSQLParameter("@FinalQty", recipePlanDetailsDTO.FinalQty));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UOMId", recipePlanDetailsDTO.UOMId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RecipeEstimationDetailId", recipePlanDetailsDTO.RecipeEstimationDetailId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@QtyModifiedDate", recipePlanDetailsDTO.QtyModifiedDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", recipePlanDetailsDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", recipePlanDetailsDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastupdatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }


        private RecipePlanDetailsDTO GetRecipePlanDetailsDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            RecipePlanDetailsDTO recipePlanDetailsDTO = new RecipePlanDetailsDTO(
                dataRow["RecipePlanDetailId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["RecipePlanDetailId"]),
                Convert.ToInt32(dataRow["RecipePlanHeaderId"]),
                Convert.ToInt32(dataRow["ProductId"]),
                dataRow["PlannedQty"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["PlannedQty"]),
                dataRow["IncrementalQty"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["IncrementalQty"]),
                dataRow["FinalQty"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["FinalQty"]),
                dataRow["UOMId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["UOMId"]),
                dataRow["RecipeEstimationDetailId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["RecipeEstimationDetailId"]),
                dataRow["QtyModifiedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["QtyModifiedDate"]),
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
            log.LogMethodExit(recipePlanDetailsDTO);
            return recipePlanDetailsDTO;
        }

        internal RecipePlanDetailsDTO GetRecipePlanDetailsDTO(int recipePlanDetailsId)
        {
            log.LogMethodEntry(recipePlanDetailsId);
            RecipePlanDetailsDTO result = null;
            string query = SELECT_QUERY + @" WHERE rpd.RecipePlanDetailId = @RecipePlanDetailId";
            SqlParameter parameter = new SqlParameter("@RecipePlanDetailId", recipePlanDetailsId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetRecipePlanDetailsDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }
        internal void Delete(RecipePlanDetailsDTO recipePlanDetailsDTO)
        {
            log.LogMethodEntry(recipePlanDetailsDTO);
            string query = @"DELETE  
                             FROM RecipePlanDetails
                             WHERE RecipePlanDetailId = @RecipePlanDetailId";
            SqlParameter parameter = new SqlParameter("@RecipePlanDetailId", recipePlanDetailsDTO.RecipePlanDetailId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit();
        }

        internal List<RecipePlanDetailsDTO> GetRecipePlanDetailsDTOListOfRecipe(List<int> recipePlanHeaderIdList, bool activeRecords)
        {
            log.LogMethodEntry(recipePlanHeaderIdList);
            List<RecipePlanDetailsDTO> recipePlanDetailsDTOList = new List<RecipePlanDetailsDTO>();
            string query = @"SELECT *
                            FROM RecipePlanDetails, @RecipePlanHeaderIdList List
                            WHERE RecipePlanHeaderId = List.Id ";
            if (activeRecords)
            {
                query += " AND isActive = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@RecipePlanHeaderIdList", recipePlanHeaderIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                recipePlanDetailsDTOList = table.Rows.Cast<DataRow>().Select(x => GetRecipePlanDetailsDTO(x)).ToList();
            }
            log.LogMethodExit(recipePlanDetailsDTOList);
            return recipePlanDetailsDTOList;
        }

        private void RefreshRecipePlanDetailsDTO(RecipePlanDetailsDTO recipePlanDetailsDTO, DataTable dt)
        {
            log.LogMethodEntry(recipePlanDetailsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                recipePlanDetailsDTO.RecipePlanDetailId = Convert.ToInt32(dt.Rows[0]["RecipePlanDetailId"]);
                recipePlanDetailsDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                recipePlanDetailsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                recipePlanDetailsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                recipePlanDetailsDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                recipePlanDetailsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                recipePlanDetailsDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        internal RecipePlanDetailsDTO Insert(RecipePlanDetailsDTO recipePlanDetailsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(recipePlanDetailsDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[RecipePlanDetails]
                               ([RecipePlanHeaderId]
                               ,[ProductId]
                               ,[PlannedQty]
                               ,[IncrementalQty]
                               ,[FinalQty]
                               ,[UOMId]
                               ,[QtyModifiedDate]
                               ,[IsActive]
                               ,[CreatedBy]
                               ,[CreationDate]
                               ,[LastUpdatedBy]
                               ,[LastUpdateDate]
                               ,[site_id]
                               ,[Guid]
                               ,[MasterEntityId])
                               
                         VALUES
                               (
                                @RecipePlanHeaderId,
                                @ProductId,
                                @PlannedQty,
                                @IncrementalQty,
                                @FinalQty,
                                @UOMId,
                                @QtyModifiedDate,
                                @IsActive,
                                @CreatedBy,
                                GETDATE(),
                                @LastUpdatedBy,
                                GETDATE(),
                                @SiteId,
                                NEWID(), 
                                @MasterEntityId
                                 )
                                SELECT * FROM RecipePlanDetails WHERE RecipePlanDetailId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(recipePlanDetailsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshRecipePlanDetailsDTO(recipePlanDetailsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(recipePlanDetailsDTO);
            return recipePlanDetailsDTO;
        }


        internal RecipePlanDetailsDTO Update(RecipePlanDetailsDTO recipePlanDetailsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(recipePlanDetailsDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[RecipePlanDetails] set 
                               [RecipePlanHeaderId]   = @RecipePlanHeaderId,
                               [ProductId]  = @ProductId,
                               [PlannedQty]   = @PlannedQty,
                               [IncrementalQty]   = @IncrementalQty,
                               [FinalQty]   = @FinalQty,
                               [UOMId]   = @UOMId,
                               [RecipeEstimationDetailId]   = @RecipeEstimationDetailId,
                               [QtyModifiedDate] = @QtyModifiedDate,
                               [IsActive]       = @IsActive,
                               [MasterEntityId] = @MasterEntityId,
                               [LastUpdatedBy]  = @LastUpdatedBy,
                               [LastUpdateDate] = GETDATE()
                               where RecipePlanDetailId = @RecipePlanDetailId
                             SELECT * FROM RecipePlanDetails WHERE RecipePlanDetailId = @RecipePlanDetailId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(recipePlanDetailsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshRecipePlanDetailsDTO(recipePlanDetailsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(recipePlanDetailsDTO);
            return recipePlanDetailsDTO;
        }


        internal List<RecipePlanDetailsDTO> GetRecipePlanDetailsDTOList(List<KeyValuePair<RecipePlanDetailsDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<RecipePlanDetailsDTO> recipePlanDetailsDTOList = new List<RecipePlanDetailsDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<RecipePlanDetailsDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == RecipePlanDetailsDTO.SearchByParameters.RECIPE_PLAN_DETAIL_ID
                            || searchParameter.Key == RecipePlanDetailsDTO.SearchByParameters.RECIPE_PLAN_HEADER_ID
                            || searchParameter.Key == RecipePlanDetailsDTO.SearchByParameters.PRODUCT_ID
                            || searchParameter.Key == RecipePlanDetailsDTO.SearchByParameters.RECIPE_ESTIMATION_DETAIL_ID
                            || searchParameter.Key == RecipePlanDetailsDTO.SearchByParameters.UOM_ID
                            || searchParameter.Key == RecipePlanDetailsDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == RecipePlanDetailsDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == RecipePlanDetailsDTO.SearchByParameters.IS_ACTIVE)
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
                recipePlanDetailsDTOList = dataTable.Rows.Cast<DataRow>().Select(x => GetRecipePlanDetailsDTO(x)).ToList();
            }
            log.LogMethodExit(recipePlanDetailsDTOList);
            return recipePlanDetailsDTOList;
        }
    }
}
