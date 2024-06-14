/********************************************************************************************
 * Project Name - Inventory
 * Description  - Data Handler object of Recipe Manufacturing Details
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By         Remarks          
 *********************************************************************************************
 *2.100.0       25-Jul-2020   Deeksha             Created for Recipe Management enhancement.
 *********************************************************************************************/
using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using System.Collections.Generic;

namespace Semnox.Parafait.Inventory.Recipe
{
    /// <summary>
    /// Recipe Manufacturing Details DataHandler
    /// </summary>
    public class RecipeManufacturingDetailsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM RecipeManufacturingDetails AS rmd ";

        private static readonly Dictionary<RecipeManufacturingDetailsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<RecipeManufacturingDetailsDTO.SearchByParameters, string>
            {
                {RecipeManufacturingDetailsDTO.SearchByParameters.RECIPE_MANUFACTURING_DETAIL_ID, "rmd.RecipeManufacturingDetailId"},
                {RecipeManufacturingDetailsDTO.SearchByParameters.RECIPE_MANUFACTURING_HEADER_ID, "rmd.RecipeManufacturingHeaderId"},
                {RecipeManufacturingDetailsDTO.SearchByParameters.MFGLINE_ID, "rmd.MfgLineId"},
                {RecipeManufacturingDetailsDTO.SearchByParameters.PRODUCT_ID, "rmd.ProductId"},
                {RecipeManufacturingDetailsDTO.SearchByParameters.PARENT_MFG_LINE_ID, "rmd.ParentMFGLineId"},
                {RecipeManufacturingDetailsDTO.SearchByParameters.TOP_MOST_PARENT_MFG_LINE_ID, "rmd.TopMostParentMFGLineId"},
                {RecipeManufacturingDetailsDTO.SearchByParameters.MFGUOM_ID, "rmd.MfgUOMId"},
                {RecipeManufacturingDetailsDTO.SearchByParameters.ACTUALMFG_UOM_ID, "rmd.ActualMfgUOMId"},
                {RecipeManufacturingDetailsDTO.SearchByParameters.RECIPE_PLAN_DETAIL_ID, "rmd.RecipePlanDetailId"},
                {RecipeManufacturingDetailsDTO.SearchByParameters.IS_ACTIVE, "rmd.IsActive"},
                {RecipeManufacturingDetailsDTO.SearchByParameters.MASTER_ENTITY_ID, "rmd.MasterEntityId"},
                {RecipeManufacturingDetailsDTO.SearchByParameters.SITE_ID, "rmd.site_id"},
            };

        /// <summary>
        /// Parameterized Constructor for RecipeManufacturingDetailsDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction Object</param>
        public RecipeManufacturingDetailsDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        private List<SqlParameter> GetSQLParameters(RecipeManufacturingDetailsDTO recipeManufacturingDetailsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(recipeManufacturingDetailsDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@RecipeManufacturingDetailId", recipeManufacturingDetailsDTO.RecipeManufacturingDetailId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RecipeManufacturingHeaderId", recipeManufacturingDetailsDTO.RecipeManufacturingHeaderId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MfgLineId", recipeManufacturingDetailsDTO.MfgLineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MfgUOMId", recipeManufacturingDetailsDTO.MfgUOMId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProductId", recipeManufacturingDetailsDTO.ProductId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsParentItem", recipeManufacturingDetailsDTO.IsParentItem));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ParentMFGLineId", recipeManufacturingDetailsDTO.ParentMFGLineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TopMostParentMFGLineId", recipeManufacturingDetailsDTO.TopMostParentMFGLineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Quantity", recipeManufacturingDetailsDTO.Quantity));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ActualMfgQuantity", recipeManufacturingDetailsDTO.ActualMfgQuantity));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ActualMfgUOMId", recipeManufacturingDetailsDTO.ActualMfgUOMId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ItemCost", recipeManufacturingDetailsDTO.ItemCost));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PlannedCost", recipeManufacturingDetailsDTO.PlannedCost));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ActualCost", recipeManufacturingDetailsDTO.ActualCost));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RecipePlanDetailId", recipeManufacturingDetailsDTO.RecipePlanDetailId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", recipeManufacturingDetailsDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", recipeManufacturingDetailsDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsComplete", recipeManufacturingDetailsDTO.IsComplete));
            log.LogMethodExit(parameters);
            return parameters;
        }

        private RecipeManufacturingDetailsDTO GetRecipeManufacturingDetailsDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            RecipeManufacturingDetailsDTO recipeManufacturingDetailsDTO = new RecipeManufacturingDetailsDTO(
                dataRow["RecipeManufacturingDetailId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["RecipeManufacturingDetailId"]),
                Convert.ToInt32(dataRow["RecipeManufacturingHeaderId"]),//Not nullable 
                Convert.ToInt32(dataRow["MfgLineId"]),
                Convert.ToInt32(dataRow["ProductId"]),
                dataRow["IsParentItem"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["IsParentItem"]),
                dataRow["ParentMFGLineId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ParentMFGLineId"]),
                dataRow["TopMostParentMFGLineId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TopMostParentMFGLineId"]),
                dataRow["Quantity"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["Quantity"]),
                dataRow["MfgUOMId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MfgUOMId"]),
                dataRow["ActualMfgQuantity"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["ActualMfgQuantity"]),
                dataRow["ActualMfgUOMId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ActualMfgUOMId"]),
                dataRow["ItemCost"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["ItemCost"]),
                dataRow["PlannedCost"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["PlannedCost"]),
                dataRow["ActualCost"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["ActualCost"]),
                dataRow["RecipePlanDetailId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["RecipePlanDetailId"]),
                dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                dataRow["IsComplete"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsComplete"])
                );
            log.LogMethodExit(recipeManufacturingDetailsDTO);
            return recipeManufacturingDetailsDTO;
        }

        internal RecipeManufacturingDetailsDTO GetRecipeManufacturingDetailsId(int recipeManufacturingDetailsId)
        {
            log.LogMethodEntry(recipeManufacturingDetailsId);
            RecipeManufacturingDetailsDTO result = null;
            string query = SELECT_QUERY + @" WHERE rmd.RecipeManufacturingDetailId = @RecipeManufacturingDetailId";
            SqlParameter parameter = new SqlParameter("@RecipeManufacturingDetailId", recipeManufacturingDetailsId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetRecipeManufacturingDetailsDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        internal void Delete(RecipeManufacturingDetailsDTO recipeManufacturingDetailsDTO)
        {
            log.LogMethodEntry(recipeManufacturingDetailsDTO);
            string query = @"DELETE  
                             FROM RecipeManufacturingDetails
                             WHERE RecipeManufacturingDetails.RecipeManufacturingDetailId = @RecipeManufacturingDetailId";
            SqlParameter parameter = new SqlParameter("@RecipeManufacturingDetailId", recipeManufacturingDetailsDTO.RecipeManufacturingDetailId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            recipeManufacturingDetailsDTO.AcceptChanges();
            log.LogMethodExit();
        }

        internal List<RecipeManufacturingDetailsDTO> GetRecipeManufacturingDetailsDTOListOfRecipe(List<int> recipeManufacturingHeaderIdList, bool activeRecords)
        {
            log.LogMethodEntry(recipeManufacturingHeaderIdList);
            List<RecipeManufacturingDetailsDTO> recipeManufacturingDetailsDTOList = new List<RecipeManufacturingDetailsDTO>();
            string query = @"SELECT *
                            FROM RecipeManufacturingDetails, @RecipeManufacturingHeaderId List
                            WHERE RecipeManufacturingHeaderId = List.Id ";
            if (activeRecords)
            {
                query += " AND isActive = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@RecipeManufacturingHeaderId", recipeManufacturingHeaderIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                recipeManufacturingDetailsDTOList = table.Rows.Cast<DataRow>().Select(x => GetRecipeManufacturingDetailsDTO(x)).ToList();
            }
            log.LogMethodExit(recipeManufacturingDetailsDTOList);
            return recipeManufacturingDetailsDTOList;
        }

        private void RefreshRecipeManufacturingDetailsDTO(RecipeManufacturingDetailsDTO recipeManufacturingDetailsDTO, DataTable dt)
        {
            log.LogMethodEntry(recipeManufacturingDetailsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                recipeManufacturingDetailsDTO.RecipeManufacturingDetailId = Convert.ToInt32(dt.Rows[0]["RecipeManufacturingDetailId"]);
                recipeManufacturingDetailsDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                recipeManufacturingDetailsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                recipeManufacturingDetailsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                recipeManufacturingDetailsDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                recipeManufacturingDetailsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                recipeManufacturingDetailsDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        internal RecipeManufacturingDetailsDTO Insert(RecipeManufacturingDetailsDTO recipeManufacturingDetailsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(recipeManufacturingDetailsDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[RecipeManufacturingDetails]
                               (  [RecipeManufacturingHeaderId],
                                  [MfgLineId],
                                  [ProductId],
                                  [IsParentItem],
                                  [ParentMFGLineId],
                                  [TopMostParentMFGLineId],
                                  [Quantity],
                                  [MfgUOMId],
                                  [ActualMfgQuantity],
                                  [ActualMfgUOMId],
                                  [ItemCost],
                                  [PlannedCost],
                                  [ActualCost],
                                  [RecipePlanDetailId],
                                  [IsActive],
                                  [IsComplete],
                                  [CreatedBy],
                                  [CreationDate],
                                  [LastUpdatedBy],
                                  [LastUpdateDate],
                                  [site_id],
                                  [Guid],
                                  [MasterEntityId])
                               
                         VALUES
                               (    @RecipeManufacturingHeaderId,
                                    @MfgLineId,
                                    @ProductId,
                                    @IsParentItem,
                                    @ParentMFGLineId,
                                    @TopMostParentMFGLineId,
                                    @Quantity,
                                    @MfgUOMId,
                                    @ActualMfgQuantity,
                                    @ActualMfgUOMId,
                                    @ItemCost,
                                    @PlannedCost,
                                    @ActualCost,
                                    @RecipePlanDetailId,
                                    @IsActive,
                                    @IsComplete,
                                    @CreatedBy,
                                    GETDATE(),
                                    @LastUpdatedBy,
                                    GETDATE(),
                                    @SiteId,
                                    NEWID(), 
                                    @MasterEntityId )
                                SELECT * FROM RecipeManufacturingDetails WHERE RecipeManufacturingDetailId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(recipeManufacturingDetailsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshRecipeManufacturingDetailsDTO(recipeManufacturingDetailsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(recipeManufacturingDetailsDTO);
            return recipeManufacturingDetailsDTO;
        }

        internal RecipeManufacturingDetailsDTO Update(RecipeManufacturingDetailsDTO recipeManufacturingDetailsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(recipeManufacturingDetailsDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[RecipeManufacturingDetails] set
                               [RecipeManufacturingHeaderId]     =@RecipeManufacturingHeaderId,
                               [MfgLineId]                       = @MfgLineId,
                               [ProductId]                       = @ProductId,
                               [IsParentItem]                    = @IsParentItem,
                               [ParentMFGLineId]                 = @ParentMFGLineId,
                               [TopMostParentMFGLineId]          = @TopMostParentMFGLineId,
                               [Quantity]                        = @Quantity,
                               [MfgUOMId]                        = @MfgUOMId,
                               [ActualMfgQuantity]               = @ActualMfgQuantity,
                               [ActualMfgUOMId]                  = @ActualMfgUOMId,
                               [ItemCost]                        = @ItemCost,
                               [PlannedCost]                     = @PlannedCost,
                               [ActualCost]                      = @ActualCost,
                               [MasterEntityId]                  = @MasterEntityId,
                               [RecipePlanDetailId]              = @RecipePlanDetailId,
                               [IsActive]                        = @IsActive,
                               [IsComplete]                      = @IsComplete,
                               [LastUpdatedBy]                   = @LastUpdatedBy,
                               [LastUpdateDate]                  = GETDATE()
                               where RecipeManufacturingDetailId = @RecipeManufacturingDetailId
                             SELECT * FROM RecipeManufacturingDetails WHERE RecipeManufacturingDetailId = @RecipeManufacturingDetailId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(recipeManufacturingDetailsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshRecipeManufacturingDetailsDTO(recipeManufacturingDetailsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(recipeManufacturingDetailsDTO);
            return recipeManufacturingDetailsDTO;
        }

        internal List<RecipeManufacturingDetailsDTO> GetRecipeManufacturingDetailsDTOList(List<KeyValuePair<RecipeManufacturingDetailsDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<RecipeManufacturingDetailsDTO> recipeManufacturingDetailsDTOList = new List<RecipeManufacturingDetailsDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<RecipeManufacturingDetailsDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == RecipeManufacturingDetailsDTO.SearchByParameters.RECIPE_MANUFACTURING_HEADER_ID ||
                            searchParameter.Key == RecipeManufacturingDetailsDTO.SearchByParameters.RECIPE_PLAN_DETAIL_ID ||
                            searchParameter.Key == RecipeManufacturingDetailsDTO.SearchByParameters.RECIPE_MANUFACTURING_DETAIL_ID ||
                            searchParameter.Key == RecipeManufacturingDetailsDTO.SearchByParameters.PRODUCT_ID ||
                            searchParameter.Key == RecipeManufacturingDetailsDTO.SearchByParameters.ACTUALMFG_UOM_ID ||
                            searchParameter.Key == RecipeManufacturingDetailsDTO.SearchByParameters.MFGLINE_ID ||
                            searchParameter.Key == RecipeManufacturingDetailsDTO.SearchByParameters.MFGUOM_ID ||
                            searchParameter.Key == RecipeManufacturingDetailsDTO.SearchByParameters.PARENT_MFG_LINE_ID ||
                            searchParameter.Key == RecipeManufacturingDetailsDTO.SearchByParameters.MASTER_ENTITY_ID ||
                            searchParameter.Key == RecipeManufacturingDetailsDTO.SearchByParameters.TOP_MOST_PARENT_MFG_LINE_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == RecipeManufacturingDetailsDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == RecipeManufacturingDetailsDTO.SearchByParameters.IS_ACTIVE)
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
                recipeManufacturingDetailsDTOList = dataTable.Rows.Cast<DataRow>().Select(x => GetRecipeManufacturingDetailsDTO(x)).ToList();
            }
            log.LogMethodExit(recipeManufacturingDetailsDTOList);
            return recipeManufacturingDetailsDTOList;
        }
    }
}
