/********************************************************************************************
 * Project Name - Inventory
 * Description  - Data Handler object of Recipe Plan Header
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By         Remarks          
 *********************************************************************************************
 *2.100.0        20-Jul-2020   Deeksha             Created for Recipe Management enhancement.
 *********************************************************************************************/
using System;
using System.Text;
using System.Data;
using System.Linq;
using System.Globalization;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using System.Collections.Generic;

namespace Semnox.Parafait.Inventory.Recipe
{
    /// <summary>
    /// Recipe Plan Header DataHandler
    /// </summary>
    public class RecipePlanHeaderDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM RecipePlanHeader AS rph ";

        private static readonly Dictionary<RecipePlanHeaderDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<RecipePlanHeaderDTO.SearchByParameters, string>
            {
                {RecipePlanHeaderDTO.SearchByParameters.RECIPE_PLAN_HEADER__ID, "rph.RecipePlanHeaderId"},
                {RecipePlanHeaderDTO.SearchByParameters.RECIPE_PLAN_HEADER_ID_LIST, "rph.RecipePlanHeaderId"},
                {RecipePlanHeaderDTO.SearchByParameters.RECIPE_ESTIMATION_HEADER_ID, "rph.RecipeEstimationHeaderId"},
                {RecipePlanHeaderDTO.SearchByParameters.IS_ACTIVE, "rph.IsActive"},
                {RecipePlanHeaderDTO.SearchByParameters.FROM_DATE, "rph.PlanDateTime"},
                {RecipePlanHeaderDTO.SearchByParameters.PLAN_FROM_DATE, "rph.PlanDateTime"},
                {RecipePlanHeaderDTO.SearchByParameters.TO_DATE, "rph.PlanDateTime"},
                {RecipePlanHeaderDTO.SearchByParameters.PLAN_TO_DATE, "rph.PlanDateTime"},
                {RecipePlanHeaderDTO.SearchByParameters.RECUR_END_DATE, "rph.RecurEndDate"},
                {RecipePlanHeaderDTO.SearchByParameters.MASTER_ENTITY_ID, "rph.MasterEntityId"},
                {RecipePlanHeaderDTO.SearchByParameters.SITE_ID, "rph.site_id"},
            };


        /// <summary>
        /// Parameterized Constructor for RecipePlanHeaderDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction Object</param>
        public RecipePlanHeaderDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        private List<SqlParameter> GetSQLParameters(RecipePlanHeaderDTO recipePlanHeaderDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(recipePlanHeaderDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@RecipePlanHeaderId", recipePlanHeaderDTO.RecipePlanHeaderId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RecipeEstimationHeaderId", recipePlanHeaderDTO.RecipeEstimationHeaderId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PlanDateTime", recipePlanHeaderDTO.PlanDateTime, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RecurFlag", recipePlanHeaderDTO.RecurFlag)); 
            parameters.Add(dataAccessHandler.GetSQLParameter("@RecurFrequency", recipePlanHeaderDTO.RecurFrequency));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RecurEndDate", recipePlanHeaderDTO.RecurEndDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RecurType", recipePlanHeaderDTO.RecurType));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Sunday", recipePlanHeaderDTO.Sunday));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Monday", recipePlanHeaderDTO.Monday));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Tuesday", recipePlanHeaderDTO.Tuesday));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Wednesday", recipePlanHeaderDTO.Wednesday, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Thursday", recipePlanHeaderDTO.Thursday, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Friday", recipePlanHeaderDTO.Friday, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Saturday", recipePlanHeaderDTO.Saturday, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", recipePlanHeaderDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", recipePlanHeaderDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }


        internal RecipePlanHeaderDTO GetRecipePlanHeaderId(int recipePlanHeaderId)
        {
            log.LogMethodEntry(recipePlanHeaderId);
            RecipePlanHeaderDTO result = null;
            string query = SELECT_QUERY + @" WHERE rph.RecipePlanHeaderId = @RecipePlanHeaderId";
            SqlParameter parameter = new SqlParameter("@RecipePlanHeaderId", recipePlanHeaderId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetRecipePlanHeaderDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        internal List<RecipePlanHeaderDTO> GetRecipePlanHedeaderDTO(DateTime fromDate , DateTime toDate , int siteId)
        {
            log.LogMethodEntry(fromDate, toDate, siteId);
            List<RecipePlanHeaderDTO> recipePlanHeaderDTOList = new List<RecipePlanHeaderDTO>();
            string query = SELECT_QUERY + @"where (site_id = @site_id or @site_id = -1) " +
                                "and ((PlanDateTime >= @fromDate and PlanDateTime < @toDate) " +
                                "and isActive = 1" +
                                    "or (PlanDateTime > @fromDate and PlanDateTime <= @toDate) " +
                                    "or (PlanDateTime < @fromDate and PlanDateTime >= @toDate) " +
                                    "or (RecurFlag = '1' and ((RecurEndDate >= @fromdate and PlanDateTime < @todate)))) " +
                                "order by PlanDateTime desc";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@fromDate", fromDate));
            parameters.Add(new SqlParameter("@toDate", toDate));
            parameters.Add(new SqlParameter("@site_id", siteId));
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                recipePlanHeaderDTOList = dataTable.Rows.Cast<DataRow>().Select(x => GetRecipePlanHeaderDTO(x)).ToList();
            }
            log.LogMethodExit(recipePlanHeaderDTOList);
            return recipePlanHeaderDTOList;
        }

        internal void Delete(RecipePlanHeaderDTO recipePlanHeaderDTO)
        {
            log.LogMethodEntry(recipePlanHeaderDTO);
            string query = @"DELETE  
                             FROM RecipePlanHeader
                             WHERE RecipePlanHeader.RecipePlanHeaderId = @RecipePlanHeaderId";
            SqlParameter parameter = new SqlParameter("@RecipePlanHeaderId", recipePlanHeaderDTO.RecipePlanHeaderId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            recipePlanHeaderDTO.AcceptChanges();
            log.LogMethodExit();
        }


        private void RefreshRecipePlanHeaderDTO(RecipePlanHeaderDTO recipePlanHeaderDTO, DataTable dt)
        {
            log.LogMethodEntry(recipePlanHeaderDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                recipePlanHeaderDTO.RecipePlanHeaderId = Convert.ToInt32(dt.Rows[0]["RecipePlanHeaderId"]);
                recipePlanHeaderDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                recipePlanHeaderDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                recipePlanHeaderDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                recipePlanHeaderDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                recipePlanHeaderDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                recipePlanHeaderDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        private RecipePlanHeaderDTO GetRecipePlanHeaderDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            RecipePlanHeaderDTO recipePlanHeaderDTO = new RecipePlanHeaderDTO(dataRow["RecipePlanHeaderId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["RecipePlanHeaderId"]),
                                                Convert.ToDateTime(dataRow["PlanDateTime"]),
                                                dataRow["RecurFlag"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["RecurFlag"]),
                                                dataRow["RecurEndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["RecurEndDate"]),
                                                dataRow["RecurFrequency"] == DBNull.Value ? (char?)null : Convert.ToChar(dataRow["RecurFrequency"]),
                                                dataRow["RecurType"] == DBNull.Value ? (char?)null : Convert.ToChar(dataRow["RecurType"]),
                                                dataRow["RecipeEstimationHeaderId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["RecipeEstimationHeaderId"]),
                                                dataRow["Sunday"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["Sunday"]),
                                                dataRow["Monday"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["Monday"]),
                                                dataRow["Tuesday"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["Tuesday"]),
                                                dataRow["Wednesday"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["Wednesday"]),
                                                dataRow["Thursday"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["Thursday"]),
                                                dataRow["Friday"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["Friday"]),
                                                dataRow["Saturday"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["Saturday"]),
                                                dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                                                dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                                dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]));
            log.LogMethodExit(recipePlanHeaderDTO);
            return recipePlanHeaderDTO;
        }

        internal RecipePlanHeaderDTO Insert(RecipePlanHeaderDTO recipePlanHeaderDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(recipePlanHeaderDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[RecipePlanHeader]
                               ([PlanDateTime]
                               ,[RecurFlag]
                               ,[RecurEndDate]
                               ,[RecurFrequency]
                               ,[RecurType]
                               ,[RecipeEstimationHeaderId]
                               ,[Sunday]
                               ,[Monday]
                               ,[Tuesday]
                               ,[Wednesday]
                               ,[Thursday]
                               ,[Friday]
                               ,[Saturday]
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
                                @PlanDateTime,
                                @RecurFlag,
                                @RecurEndDate,
                                @RecurFrequency,
                                @RecurType,
                                @RecipeEstimationHeaderId,
                                @Sunday,
                                @Monday,
                                @Tuesday,
                                @Wednesday,
                                @Thursday,
                                @Friday,
                                @Saturday,
                                @IsActive,
                                @CreatedBy,
                                GETDATE(),
                                @LastUpdatedBy,
                                GETDATE(),
                                @SiteId,
                                NEWID(), 
                                @MasterEntityId
                                 )
                                SELECT * FROM RecipePlanHeader WHERE RecipePlanHeaderId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(recipePlanHeaderDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshRecipePlanHeaderDTO(recipePlanHeaderDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(recipePlanHeaderDTO);
            return recipePlanHeaderDTO;
        }

        internal RecipePlanHeaderDTO Update(RecipePlanHeaderDTO recipePlanHeaderDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(recipePlanHeaderDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[RecipePlanHeader] set
                               [PlanDateTime]               = @PlanDateTime,
                               [RecurFlag]                  = @RecurFlag,
                               [RecurEndDate]               = @RecurEndDate,
                               [RecurType]                  = @RecurType,
                               [RecurFrequency]             = @RecurFrequency,
                               [RecipeEstimationHeaderId]   = @RecipeEstimationHeaderId,
                               [Sunday]                     = @Sunday,
                               [Monday]                     = @Monday,
                               [Tuesday]                    = @Tuesday,
                               [Wednesday]                  = @Wednesday,
                               [Thursday]                   = @Thursday,
                               [Friday]                     = @Friday,
                               [MasterEntityId]             = @MasterEntityId,
                               [Saturday]                   = @Saturday,
                               [IsActive]                   = @IsActive,
                               [LastUpdatedBy]              = @LastUpdatedBy,
                               [LastUpdateDate]             = GETDATE()
                               where RecipePlanHeaderId = @RecipePlanHeaderId
                             SELECT * FROM RecipePlanHeader WHERE RecipePlanHeaderId = @RecipePlanHeaderId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(recipePlanHeaderDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshRecipePlanHeaderDTO(recipePlanHeaderDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(recipePlanHeaderDTO);
            return recipePlanHeaderDTO;
        }

        internal List<RecipePlanHeaderDTO> GetRecipePlanHeaderDTOList(List<KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<RecipePlanHeaderDTO> recipePlanHeaderDTOList = new List<RecipePlanHeaderDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == RecipePlanHeaderDTO.SearchByParameters.RECIPE_PLAN_HEADER__ID ||
                            searchParameter.Key == RecipePlanHeaderDTO.SearchByParameters.RECIPE_ESTIMATION_HEADER_ID ||
                            searchParameter.Key == RecipePlanHeaderDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == RecipePlanHeaderDTO.SearchByParameters.RECUR_END_DATE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == RecipePlanHeaderDTO.SearchByParameters.FROM_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == RecipePlanHeaderDTO.SearchByParameters.TO_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) <= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == RecipePlanHeaderDTO.SearchByParameters.PLAN_FROM_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == RecipePlanHeaderDTO.SearchByParameters.PLAN_TO_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == RecipePlanHeaderDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == RecipePlanHeaderDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == RecipePlanHeaderDTO.SearchByParameters.RECIPE_PLAN_HEADER_ID_LIST)
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
                recipePlanHeaderDTOList = dataTable.Rows.Cast<DataRow>().Select(x => GetRecipePlanHeaderDTO(x)).ToList();
            }
            log.LogMethodExit(recipePlanHeaderDTOList);
            return recipePlanHeaderDTOList;
        }
    }
}
