/* Project Name - Semnox.Parafait.Booking.ScheduleRulesDataHandler 
* Description  - Data handler object of the AttractionScheduleRules
* 
**************
**Version Log
**************
*Version     Date           Modified By             Remarks          
********************************************************************************************* 
*2.50        26-Nov-2018    Guru S A             Created for Booking enhancement changes 
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Booking
{
    public class ScheduleRulesDataHandler
    {
        DataAccessHandler dataAccessHandler;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        private static readonly Dictionary<ScheduleRulesDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ScheduleRulesDTO.SearchByParameters, string>
            {
                {ScheduleRulesDTO.SearchByParameters.SCHEDULE_RULE_ID, "Id"},
                {ScheduleRulesDTO.SearchByParameters.SCHEDULE_ID, "AttractionScheduleId"},
                {ScheduleRulesDTO.SearchByParameters.PRODUCT_ID, "ProductId"}, 
                {ScheduleRulesDTO.SearchByParameters.MASTER_ENTITY_ID, "MasterEntityId"},
                {ScheduleRulesDTO.SearchByParameters.SITE_ID, "site_id"} 
            };

        private static readonly string atSRSelectQuery = @"SELECT *
                                                            FROM AttractionScheduleRules ";

        /// <summary>
        /// Default constructor of  ScheduleRulesDataHandler class
        /// </summary>
        public ScheduleRulesDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating scheduleRules Record.
        /// </summary>
        /// <param name="scheduleRulesDTO">ScheduleRulesDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(ScheduleRulesDTO scheduleRulesDTO, string userId, int siteId)
        { 
            log.LogMethodEntry(scheduleRulesDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@AttractionScheduleRulesId", scheduleRulesDTO.ScheduleRulesId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@AttractionScheduleId", scheduleRulesDTO.ScheduleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Day", scheduleRulesDTO.Day));
            parameters.Add(dataAccessHandler.GetSQLParameter("@FromDate", scheduleRulesDTO.FromDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ToDate", scheduleRulesDTO.ToDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Units", scheduleRulesDTO.Units));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Price", scheduleRulesDTO.Price));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProductId", scheduleRulesDTO.ProductId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", scheduleRulesDTO.MasterEntityId, true)); 
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", userId)); 
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the ScheduleRules record to the database
        /// </summary>
        /// <param name="scheduleRulesDTO">ScheduleRulesDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertScheduleRules(ScheduleRulesDTO scheduleRulesDTO, string userId, int siteId)
        {
            log.LogMethodEntry(scheduleRulesDTO, userId, siteId);
            int idOfRowInserted;
            string query = @"INSERT INTO dbo.AttractionScheduleRules
                                        (AttractionScheduleId
                                        ,Day
                                        ,FromDate
                                        ,Todate
                                        ,Units
                                        ,Price
                                        ,site_id
                                        ,Guid 
                                        ,ProductId
                                        ,MasterEntityId
                                        ,CreatedBy
                                        ,CreationDate
                                        ,LastUpdatedBy
                                        ,LastUpdateDate)
                                    VALUES
                                        (@AttractionScheduleId
                                        ,@Day
                                        ,@FromDate
                                        ,@Todate
                                        ,@Units
                                        ,@Price
                                        ,@site_id
                                        ,NEWID() 
                                        ,@ProductId 
                                        ,@MasterEntityId 
                                        ,@CreatedBy 
                                        ,getdate()
                                        ,@LastUpdatedBy 
                                        ,getdate()
                            )SELECT CAST(scope_identity() AS int)";
            try
            {
                idOfRowInserted = dataAccessHandler.executeInsertQuery(query, GetSQLParameters(scheduleRulesDTO, userId, siteId).ToArray(), sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(idOfRowInserted);
            return idOfRowInserted;
        }

        /// <summary>
        /// Updates the ScheduleRules record
        /// </summary>
        /// <param name="scheduleRulesDTO">ScheduleRulesDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public int UpdateScheduleRules(ScheduleRulesDTO scheduleRulesDTO, string userId, int siteId)
        {
            log.LogMethodEntry(scheduleRulesDTO, userId, siteId);
            int rowsUpdated;
            string query = @"
                            UPDATE AttractionScheduleRules
                               SET AttractionScheduleId = @AttractionScheduleId
                                  ,Day = @Day
                                  ,FromDate = @FromDate
                                  ,Todate = @Todate
                                  ,Units = @Units
                                  ,Price = @Price
                                  --,site_id = @site_id 
                                  ,ProductId = @ProductId
                                  ,MasterEntityId = @MasterEntityId 
                                  ,LastUpdatedBy = @LastUpdatedBy
                                  ,LastUpdateDate = getdate()
                             WHERE  id = @AttractionScheduleRulesId";
            try
            {
                rowsUpdated = dataAccessHandler.executeUpdateQuery(query, GetSQLParameters(scheduleRulesDTO, userId, siteId).ToArray(), sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(rowsUpdated);
            return rowsUpdated;
        }



        /// <summary>
        /// Converts the Data row object to GetScheduleRulesDTO calss type
        /// </summary>
        /// <param name="scheduleRuleRow">ScheduleRules DataRow</param>
        /// <returns>Returns ScheduleRulesDTO</returns>
        private ScheduleRulesDTO GetScheduleRulesDTO(DataRow scheduleRuleRow)
        {
            log.LogMethodEntry(scheduleRuleRow); 
            ScheduleRulesDTO scheduleRulesDTO = new ScheduleRulesDTO(
                                                                    Convert.ToInt32(scheduleRuleRow["id"]),
                                                                    Convert.ToInt32(scheduleRuleRow["AttractionScheduleId"]),
                                                                    string.IsNullOrEmpty(scheduleRuleRow["day"].ToString()) ? (decimal?)null : Convert.ToDecimal(scheduleRuleRow["day"]),
                                                                    scheduleRuleRow["fromDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(scheduleRuleRow["fromDate"]),
                                                                    scheduleRuleRow["toDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(scheduleRuleRow["toDate"]),
                                                                    string.IsNullOrEmpty(scheduleRuleRow["units"].ToString()) ? (int?)null : Convert.ToInt32(scheduleRuleRow["units"]),
                                                                    string.IsNullOrEmpty(scheduleRuleRow["price"].ToString()) ? (decimal?)null : Convert.ToDecimal(scheduleRuleRow["price"]), 
                                                                    string.IsNullOrEmpty(scheduleRuleRow["site_id"].ToString()) ? -1 : Convert.ToInt32(scheduleRuleRow["site_id"]),
                                                                    scheduleRuleRow["Guid"].ToString(),
                                                                    string.IsNullOrEmpty(scheduleRuleRow["SynchStatus"].ToString()) ? false : Convert.ToBoolean(scheduleRuleRow["SynchStatus"]),
                                                                    string.IsNullOrEmpty(scheduleRuleRow["productId"].ToString()) ? -1 : Convert.ToInt32(scheduleRuleRow["productId"]),
                                                                    string.IsNullOrEmpty(scheduleRuleRow["MasterEntityId"].ToString()) ? -1 : Convert.ToInt32(scheduleRuleRow["MasterEntityId"]),
                                                                    string.IsNullOrEmpty(scheduleRuleRow["CreatedBy"].ToString()) ? "" : Convert.ToString(scheduleRuleRow["CreatedBy"]),
                                                                    scheduleRuleRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(scheduleRuleRow["CreationDate"]),
                                                                    string.IsNullOrEmpty(scheduleRuleRow["LastUpdateBy"].ToString())? "" : Convert.ToString(scheduleRuleRow["LastUpdateBy"]),
                                                                    scheduleRuleRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(scheduleRuleRow["LastUpdateDate"])
                                                                    );
            log.LogMethodExit(scheduleRulesDTO);
            return scheduleRulesDTO;
        }

        /// <summary>
        /// Gets the ScheduleRules data of passed scheduleRules Id
        /// </summary>
        /// <param name="scheduleRulesId">integer type parameter</param>
        /// <returns>Returns ScheduleRulesDTO</returns>
        public ScheduleRulesDTO GetScheduleRulesDTO(int scheduleRulesId)
        {
            log.LogMethodEntry(scheduleRulesId);
            string selectAttractionScheduleRulesQuery = atSRSelectQuery + "  WHERE id = @attractionScheduleRulesId";
            SqlParameter[] selectAttractionScheduleRulesParameters = new SqlParameter[1];
            selectAttractionScheduleRulesParameters[0] = new SqlParameter("@attractionScheduleRulesId", scheduleRulesId);
            DataTable attractionScheduleRules = dataAccessHandler.executeSelectQuery(selectAttractionScheduleRulesQuery, selectAttractionScheduleRulesParameters, sqlTransaction);
            ScheduleRulesDTO attractionScheduleRulesDataObject = new ScheduleRulesDTO();
            if (attractionScheduleRules.Rows.Count > 0)
            {
                DataRow AttractionScheduleRulesRow = attractionScheduleRules.Rows[0];
                attractionScheduleRulesDataObject = GetScheduleRulesDTO(AttractionScheduleRulesRow);
            }
            log.LogMethodExit(attractionScheduleRulesDataObject);
            return attractionScheduleRulesDataObject;
        }

        /// <summary>
        /// Gets the ScheduleRulesDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ScheduleRulesDTO matching the search criteria</returns>
        public List<ScheduleRulesDTO> GetScheduleRulesDTOList(List<KeyValuePair<ScheduleRulesDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<ScheduleRulesDTO> list = new List<ScheduleRulesDTO>(); 
            int count = 0;
            string selectQuery = atSRSelectQuery;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<ScheduleRulesDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == ScheduleRulesDTO.SearchByParameters.SCHEDULE_ID ||
                            searchParameter.Key == ScheduleRulesDTO.SearchByParameters.SCHEDULE_RULE_ID ||
                            searchParameter.Key == ScheduleRulesDTO.SearchByParameters.PRODUCT_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value);
                        }
                        else if (searchParameter.Key == ScheduleRulesDTO.SearchByParameters.SITE_ID ||
                                 searchParameter.Key == ScheduleRulesDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                        } 
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + " N'%" + searchParameter.Value + "%'");
                        }
                        count++;
                    }
                    else
                    {
                        log.LogMethodExit(null, "throwing exception");
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, null, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            { 
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ScheduleRulesDTO scheduleRulesDTO = GetScheduleRulesDTO(dataRow);
                    list.Add(scheduleRulesDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        } 
    }
}
