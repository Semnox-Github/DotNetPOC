/********************************************************************************************
 * Project Name - Inventory
 * Description  - Data Handler object of RecipeManufacturingHeader
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By         Remarks          
 *********************************************************************************************
 *2.100.0        23-Jul-2020   Deeksha             Created for Recipe Management enhancement.
 *********************************************************************************************/
using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using Semnox.Core.Utilities;
using System.Collections.Generic;

namespace Semnox.Parafait.Inventory.Recipe
{
    /// <summary>
    /// Recipe Manufacturing Header DataHandler
    /// </summary>
    public class RecipeManufacturingHeaderDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM RecipeManufacturingHeader AS rpMh ";

        private static readonly Dictionary<RecipeManufacturingHeaderDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<RecipeManufacturingHeaderDTO.SearchByParameters, string>
            {
                {RecipeManufacturingHeaderDTO.SearchByParameters.RECIPE_MANUFACTURING_HEADER_ID, "rpMh.RecipeManufacturingHeaderId"},
                {RecipeManufacturingHeaderDTO.SearchByParameters.RECIPE_MANUFACTURING_HEADER_ID_LIST, "rpMh.RecipeManufacturingHeaderId"},
                {RecipeManufacturingHeaderDTO.SearchByParameters.RECIPE_PLAN_HEADER_ID, "rpMh.RecipePlanHeaderId"},
                {RecipeManufacturingHeaderDTO.SearchByParameters.MFG_FROM_DATETIME, "rpMh.MFGDateTime"},
                {RecipeManufacturingHeaderDTO.SearchByParameters.MFG_TO_DATETIME, "rpMh.MFGDateTime"},
                {RecipeManufacturingHeaderDTO.SearchByParameters.IS_ACTIVE, "rpMh.IsActive"},
                {RecipeManufacturingHeaderDTO.SearchByParameters.MASTER_ENTITY_ID, "rpMh.MasterEntityId"},
                {RecipeManufacturingHeaderDTO.SearchByParameters.SITE_ID, "rpMh.site_id"},
            };

        /// <summary>
        /// Parameterized Constructor for RecipeManufacturingHeaderDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction Object</param>
        public RecipeManufacturingHeaderDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        private List<SqlParameter> GetSQLParameters(RecipeManufacturingHeaderDTO recipeManufacturingHeaderDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(recipeManufacturingHeaderDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@RecipeManufacturingHeaderId", recipeManufacturingHeaderDTO.RecipeManufacturingHeaderId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RecipeMFGNumber", recipeManufacturingHeaderDTO.RecipeMFGNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RecipePlanHeaderId", recipeManufacturingHeaderDTO.RecipePlanHeaderId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MFGDateTime", recipeManufacturingHeaderDTO.MFGDateTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", recipeManufacturingHeaderDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsComplete", recipeManufacturingHeaderDTO.IsComplete));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", recipeManufacturingHeaderDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }

        private RecipeManufacturingHeaderDTO GetRecipeManufacturingHeaderDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            RecipeManufacturingHeaderDTO recipeManufacturingHeaderDTO = new RecipeManufacturingHeaderDTO(dataRow["RecipeManufacturingHeaderId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["RecipeManufacturingHeaderId"]),
                                                dataRow["RecipeMFGNumber"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["RecipeMFGNumber"]),
                                                Convert.ToDateTime(dataRow["MFGDateTime"]),
                                                dataRow["IsComplete"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["IsComplete"]),
                                                dataRow["RecipePlanHeaderId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["RecipePlanHeaderId"]),
                                                dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                                                dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                                dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]));
            log.LogMethodExit(recipeManufacturingHeaderDTO);
            return recipeManufacturingHeaderDTO;
        }

        internal RecipeManufacturingHeaderDTO GetRecipeManufacturingHeaderId(int recipeManufacturingHeaderId)
        {
            log.LogMethodEntry(recipeManufacturingHeaderId);
            RecipeManufacturingHeaderDTO result = null;
            string query = SELECT_QUERY + @" WHERE rpMh.RecipeManufacturingHeaderId = @RecipeManufacturingHeaderId";
            SqlParameter parameter = new SqlParameter("@RecipeManufacturingHeaderId", recipeManufacturingHeaderId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetRecipeManufacturingHeaderDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        internal void Delete(RecipeManufacturingHeaderDTO recipeManufacturingHeaderDTO)
        {
            log.LogMethodEntry(recipeManufacturingHeaderDTO);
            string query = @"DELETE  
                             FROM RecipeManufacturingHeader
                             WHERE RecipeManufacturingHeader.RecipeManufacturingHeaderId = @RecipeManufacturingHeaderId";
            SqlParameter parameter = new SqlParameter("@RecipeManufacturingHeaderId", recipeManufacturingHeaderDTO.RecipeManufacturingHeaderId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            recipeManufacturingHeaderDTO.AcceptChanges();
            log.LogMethodExit();
        }

        internal void RefreshRecipeManufacturingHeaderDTO(RecipeManufacturingHeaderDTO recipeManufacturingHeaderDTO, DataTable dt)
        {
            log.LogMethodEntry(recipeManufacturingHeaderDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                recipeManufacturingHeaderDTO.RecipeManufacturingHeaderId = Convert.ToInt32(dt.Rows[0]["RecipeManufacturingHeaderId"]);
                recipeManufacturingHeaderDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                recipeManufacturingHeaderDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                recipeManufacturingHeaderDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                recipeManufacturingHeaderDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                recipeManufacturingHeaderDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                recipeManufacturingHeaderDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// GetNextSeqNo
        /// </summary>
        /// <param name="sequenceName">sequenceName Parameter</param>
        /// <returns>sequence No</returns>
        public string GetNextSeqNo(string sequenceName)
        {
            log.LogMethodEntry(sequenceName);
            DataTable dTable = dataAccessHandler.executeSelectQuery(@"declare @value varchar(20)
                                exec GetNextSeqValue N'" + sequenceName + "', @value out, -1 "
                                   + " select @value", null, sqlTransaction);
            try
            {
                if (dTable != null && dTable.Rows.Count > 0)
                {
                    object o = dTable.Rows[0][0];
                    if (o != null)
                    {
                        log.LogMethodExit(o);
                        return (o.ToString());
                    }
                    else
                    {
                        log.LogMethodExit(-1);
                        return "-1";
                    }
                }

            }
            catch
            {
                log.LogMethodExit(-1);
                return "-1";
            }
            log.LogMethodExit("-1");
            return "-1";

        }

        internal RecipeManufacturingHeaderDTO Insert(RecipeManufacturingHeaderDTO recipeManufacturingHeaderDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(recipeManufacturingHeaderDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[RecipeManufacturingHeader]
                               ([MFGDateTime]
                               ,[RecipeMFGNumber]
                               ,[RecipePlanHeaderId]
                               ,[IsComplete]
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
                                @MFGDateTime,
                                @RecipeMFGNumber,
                                @RecipePlanHeaderId,
                                @IsComplete,
                                @IsActive,
                                @CreatedBy,
                                GETDATE(),
                                @LastUpdatedBy,
                                GETDATE(),
                                @SiteId,
                                NEWID(), 
                                @MasterEntityId
                                 )
                                SELECT * FROM RecipeManufacturingHeader WHERE RecipeManufacturingHeaderId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(recipeManufacturingHeaderDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshRecipeManufacturingHeaderDTO(recipeManufacturingHeaderDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(recipeManufacturingHeaderDTO);
            return recipeManufacturingHeaderDTO;
        }


        internal RecipeManufacturingHeaderDTO Update(RecipeManufacturingHeaderDTO recipeManufacturingHeaderDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(recipeManufacturingHeaderDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[RecipeManufacturingHeader] set
                               [RecipeMFGNumber]            = @RecipeMFGNumber,
                               [RecipePlanHeaderId]         = @RecipePlanHeaderId,
                               [IsComplete]                 = @IsComplete,
                               [IsActive]                   = @IsActive,
                               [LastUpdatedBy]              = @LastUpdatedBy,
                               [MasterEntityId]             = @MasterEntityId,
                               [LastUpdateDate]             = GETDATE()
                               where RecipeManufacturingHeaderId = @RecipeManufacturingHeaderId
                             SELECT * FROM RecipeManufacturingHeader WHERE RecipeManufacturingHeaderId = @RecipeManufacturingHeaderId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(recipeManufacturingHeaderDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshRecipeManufacturingHeaderDTO(recipeManufacturingHeaderDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(recipeManufacturingHeaderDTO);
            return recipeManufacturingHeaderDTO;
        }

        internal List<RecipeManufacturingHeaderDTO> GetRecipeManufacturingHeaderDTOList(List<KeyValuePair<RecipeManufacturingHeaderDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<RecipeManufacturingHeaderDTO> recipeManufacturingHeaderDTOList = new List<RecipeManufacturingHeaderDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<RecipeManufacturingHeaderDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == RecipeManufacturingHeaderDTO.SearchByParameters.RECIPE_MANUFACTURING_HEADER_ID ||
                            searchParameter.Key == RecipeManufacturingHeaderDTO.SearchByParameters.RECIPE_PLAN_HEADER_ID ||
                            searchParameter.Key == RecipeManufacturingHeaderDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == RecipeManufacturingHeaderDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == RecipeManufacturingHeaderDTO.SearchByParameters.MFG_FROM_DATETIME)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == RecipeManufacturingHeaderDTO.SearchByParameters.MFG_TO_DATETIME)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) < " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == RecipeManufacturingHeaderDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == RecipeManufacturingHeaderDTO.SearchByParameters.RECIPE_MANUFACTURING_HEADER_ID_LIST)
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
            if (dataTable.Rows.Count > 0)
            {
                recipeManufacturingHeaderDTOList = dataTable.Rows.Cast<DataRow>().Select(x => GetRecipeManufacturingHeaderDTO(x)).ToList();
            }
            log.LogMethodExit(recipeManufacturingHeaderDTOList);
            return recipeManufacturingHeaderDTOList;
        }
    }
}
