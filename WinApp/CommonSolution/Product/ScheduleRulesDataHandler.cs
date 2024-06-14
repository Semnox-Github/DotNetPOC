/* Project Name - Semnox.Parafait.Booking.ScheduleRulesDataHandler 
* Description  - Data handler object of the AttractionScheduleRules
* 
**************
**Version Log
**************
*Version     Date           Modified By             Remarks          
********************************************************************************************* 
*2.50        26-Nov-2018    Guru S A             Created for Booking enhancement changes 
*2.60        27-Feb-2019    Nagesh Badiger       Added isActive Parameter
*2.70        14-Mar-2019    Guru S A             Booking phase 2 enhancement changes 
*2.70        27-Jun-2019    Akshay Gulaganji     Added DeleteScheduleRules() method
*2.70.2      10-Dec-2019    Jinto Thomas         Removed siteid from update query
*2.80.0      21-02-2020     Girish Kundar        Modified : 3 tier Changes for REST API
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Product
{
    public class ScheduleRulesDataHandler
    {
        private DataAccessHandler dataAccessHandler;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private static readonly ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        private static readonly Dictionary<ScheduleRulesDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ScheduleRulesDTO.SearchByParameters, string>
            {
                {ScheduleRulesDTO.SearchByParameters.SCHEDULE_RULE_ID, "Id"},
                {ScheduleRulesDTO.SearchByParameters.SCHEDULE_ID, "AttractionScheduleId"},
                {ScheduleRulesDTO.SearchByParameters.SCHEDULE_ID_LIST, "AttractionScheduleId"},
                {ScheduleRulesDTO.SearchByParameters.MASTER_SCHEDULE_ID, ""},
                {ScheduleRulesDTO.SearchByParameters.FACILITY_MAP_ID, "FacilityMapId"},
                {ScheduleRulesDTO.SearchByParameters.MASTER_ENTITY_ID, "MasterEntityId"},
                {ScheduleRulesDTO.SearchByParameters.SITE_ID, "site_id"},
                {ScheduleRulesDTO.SearchByParameters.IS_ACTIVE, "IsActive"}
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
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(ScheduleRulesDTO scheduleRulesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(scheduleRulesDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@AttractionScheduleRulesId", scheduleRulesDTO.ScheduleRulesId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@AttractionScheduleId", scheduleRulesDTO.ScheduleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Day", scheduleRulesDTO.Day));
            parameters.Add(dataAccessHandler.GetSQLParameter("@FromDate", scheduleRulesDTO.FromDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ToDate", scheduleRulesDTO.ToDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Units", scheduleRulesDTO.Units));
            //parameters.Add(dataAccessHandler.GetSQLParameter("@Price", scheduleRulesDTO.Price));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            //parameters.Add(dataAccessHandler.GetSQLParameter("@ProductId", scheduleRulesDTO.ProductId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@FacilityMapId", scheduleRulesDTO.FacilityMapId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", scheduleRulesDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", scheduleRulesDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the ScheduleRules record to the database
        /// </summary>
        /// <param name="scheduleRulesDTO">ScheduleRulesDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public ScheduleRulesDTO InsertScheduleRules(ScheduleRulesDTO scheduleRulesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(scheduleRulesDTO, loginId, siteId);
            string query = @"INSERT INTO dbo.AttractionScheduleRules
                                        (AttractionScheduleId
                                        ,Day
                                        ,FromDate
                                        ,Todate
                                        ,Units 
                                        ,site_id
                                        ,Guid 
                                        ,FacilityMapId
                                        ,MasterEntityId
                                        ,CreatedBy
                                        ,CreationDate
                                        ,LastUpdatedBy
                                        ,LastUpdateDate
                                        ,IsActive)
                                    VALUES
                                        (@AttractionScheduleId
                                        ,@Day
                                        ,@FromDate
                                        ,@Todate
                                        ,@Units 
                                        ,@site_id
                                        ,NEWID() 
                                        ,@FacilityMapId 
                                        ,@MasterEntityId 
                                        ,@CreatedBy 
                                        ,getdate()
                                        ,@LastUpdatedBy 
                                        ,getdate()
                                        ,@isActive
                            ) SELECT * FROM AttractionScheduleRules WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(scheduleRulesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshScheduleRulesDTO(scheduleRulesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting scheduleRulesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(scheduleRulesDTO);
            return scheduleRulesDTO;
        }

        private void RefreshScheduleRulesDTO(ScheduleRulesDTO scheduleRulesDTO, DataTable dt)
        {
            log.LogMethodEntry(scheduleRulesDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                scheduleRulesDTO.ScheduleRulesId = Convert.ToInt32(dt.Rows[0]["Id"]);
                scheduleRulesDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                scheduleRulesDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                scheduleRulesDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                scheduleRulesDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                scheduleRulesDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString();
                scheduleRulesDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Updates the ScheduleRules record
        /// </summary>
        /// <param name="scheduleRulesDTO">ScheduleRulesDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public ScheduleRulesDTO UpdateScheduleRules(ScheduleRulesDTO scheduleRulesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(scheduleRulesDTO, loginId, siteId);
            string query = @"
                            UPDATE AttractionScheduleRules
                               SET AttractionScheduleId = @AttractionScheduleId
                                  ,Day = @Day
                                  ,FromDate = @FromDate
                                  ,Todate = @Todate
                                  ,Units = @Units 
                                  ,FacilityMapId = @FacilityMapId
                                  ,MasterEntityId = @MasterEntityId 
                                  ,LastUpdatedBy = @LastUpdatedBy
                                  ,LastUpdateDate = getdate()
                                  ,IsActive=@isActive
                             WHERE  id = @AttractionScheduleRulesId 
                       SELECT * FROM AttractionScheduleRules WHERE Id = @AttractionScheduleRulesId ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(scheduleRulesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshScheduleRulesDTO(scheduleRulesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating scheduleRulesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(scheduleRulesDTO);
            return scheduleRulesDTO;
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
                                                                    string.IsNullOrEmpty(scheduleRuleRow["site_id"].ToString()) ? -1 : Convert.ToInt32(scheduleRuleRow["site_id"]),
                                                                    scheduleRuleRow["Guid"].ToString(),
                                                                    string.IsNullOrEmpty(scheduleRuleRow["SynchStatus"].ToString()) ? false : Convert.ToBoolean(scheduleRuleRow["SynchStatus"]),
                                                                    string.IsNullOrEmpty(scheduleRuleRow["MasterEntityId"].ToString()) ? -1 : Convert.ToInt32(scheduleRuleRow["MasterEntityId"]),
                                                                    string.IsNullOrEmpty(scheduleRuleRow["CreatedBy"].ToString()) ? string.Empty : Convert.ToString(scheduleRuleRow["CreatedBy"]),
                                                                    scheduleRuleRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(scheduleRuleRow["CreationDate"]),
                                                                    string.IsNullOrEmpty(scheduleRuleRow["LastUpdatedBy"].ToString()) ? string.Empty : Convert.ToString(scheduleRuleRow["LastUpdatedBy"]),
                                                                    scheduleRuleRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(scheduleRuleRow["LastUpdateDate"]),
                                                                    string.IsNullOrEmpty(scheduleRuleRow["FacilityMapId"].ToString()) ? -1 : Convert.ToInt32(scheduleRuleRow["FacilityMapId"]),
                                                                    scheduleRuleRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(scheduleRuleRow["IsActive"])
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
            List<SqlParameter> parameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<ScheduleRulesDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key == ScheduleRulesDTO.SearchByParameters.SCHEDULE_ID ||
                            searchParameter.Key == ScheduleRulesDTO.SearchByParameters.SCHEDULE_RULE_ID ||
                            searchParameter.Key == ScheduleRulesDTO.SearchByParameters.FACILITY_MAP_ID ||
                            searchParameter.Key == ScheduleRulesDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ScheduleRulesDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ScheduleRulesDTO.SearchByParameters.MASTER_SCHEDULE_ID)
                        {
                            query.Append(joiner + @" exists (SELECT 1 
                                                               from AttractionSchedules ats
                                                              where AttractionScheduleRules.AttractionScheduleId = ats.AttractionScheduleId
                                                                and ats.AttractionMasterScheduleId = " + dataAccessHandler.GetParameterName(searchParameter.Key) + ")");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ScheduleRulesDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
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
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
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

        /// <summary>
        /// Based on the scheduleRulesId, appropriate AttractionScheduleRules record will be deleted
        /// </summary>
        /// <param name="scheduleRulesId">scheduleRulesId</param>
        /// <returns>return the int </returns>
        public int DeleteScheduleRules(int scheduleRulesId)
        {
            log.LogMethodEntry(scheduleRulesId);
            try
            {
                string deleteQuery = @"delete from AttractionScheduleRules where Id = @id";
                SqlParameter[] deleteParameters = new SqlParameter[1];
                deleteParameters[0] = new SqlParameter("@id", scheduleRulesId);

                int deleteStatus = dataAccessHandler.executeUpdateQuery(deleteQuery, deleteParameters, sqlTransaction);
                log.LogMethodExit(deleteStatus);
                return deleteStatus;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }
    }
}
