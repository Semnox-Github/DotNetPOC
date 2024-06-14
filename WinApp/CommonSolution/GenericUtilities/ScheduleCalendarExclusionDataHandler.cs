/********************************************************************************************
 * Project Name - Schedule Exclusion Data Handler
 * Description  - Data handler of the Schedule Exclusion class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        13-Jan-2016   Raghuveera          Created 
 *********************************************************************************************
 *1.00        18-Jul-2016   Raghuveera          Modified 
 *2.70        08-Mar-2019   Guru S A            Renamed ScheduleExclusionDataHandler as ScheduleCalendarExclusionDataHandler
 *2.70.2        26-Jul-2019   Dakshakh Raj        Modified : added GetSQLParameters(), 
 *                                                         SQL injection Issue Fix.
 *2.70.2        06-Dec-2019   Jinto Thomas            Removed siteid from update query                                                          
 *2.90         11-Aug-2020   Mushahid Faizan     Modified : Added GetScheduleCalendarExclusionDTOList() and changed default isActive value to true.
  *2.100.0     31-Aug-2020   Mushahid Faizan   siteId changes in GetSQLParameters().
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;
using Semnox.Parafait.Site;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Schedule Exclusion Data Handler - Handles insert, update and select of schedule exclusion data objects
    /// </summary>
    public class ScheduleCalendarExclusionDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction = null;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM Schedule_ExclusionDays as se";
        private ExecutionContext executionContext;

        /// <summary>
        /// Dictionary for searching Parameters for the ScheduleCalendarExclusion object.
        /// </summary>
        private static readonly Dictionary<ScheduleCalendarExclusionDTO.SearchByScheduleCalendarExclusionParameters, string> DBSearchParameters = new Dictionary<ScheduleCalendarExclusionDTO.SearchByScheduleCalendarExclusionParameters, string>
            {
                {ScheduleCalendarExclusionDTO.SearchByScheduleCalendarExclusionParameters.SCHEDULE_EXCLUSION_ID, "se.ScheduleExclusionId"},
                {ScheduleCalendarExclusionDTO.SearchByScheduleCalendarExclusionParameters.SCHEDULE_ID, "se.ScheduleId"},
                {ScheduleCalendarExclusionDTO.SearchByScheduleCalendarExclusionParameters.EXCLUSION_DATE, "se.ExclusionDate"},
                {ScheduleCalendarExclusionDTO.SearchByScheduleCalendarExclusionParameters.IS_ACTIVE, "se.IsActive"},
                {ScheduleCalendarExclusionDTO.SearchByScheduleCalendarExclusionParameters.MASTER_ENTITY_ID, "se.MasterEntityId"},
                {ScheduleCalendarExclusionDTO.SearchByScheduleCalendarExclusionParameters.SITE_ID, "se.site_id"}
            };

        /// <summary>
        /// Default constructor of ScheduleExclusionDataHandler class
        /// </summary>
        public ScheduleCalendarExclusionDataHandler(ExecutionContext executionContext,SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            this.executionContext = executionContext;
            dataAccessHandler = new  DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        ///Builds the SQL Parameter list used for inserting and updating ApplicationContent Reecord.
        /// </summary>
        /// <param name="scheduleCalendarExclusionDTO">scheduleCalendarExclusionDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns> Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(ScheduleCalendarExclusionDTO scheduleCalendarExclusionDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(scheduleCalendarExclusionDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@scheduleExclusionId", scheduleCalendarExclusionDTO.ScheduleExclusionId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@scheduleId", scheduleCalendarExclusionDTO.ScheduleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@exclusionDate", string.IsNullOrEmpty(scheduleCalendarExclusionDTO.ExclusionDate) ? DBNull.Value : (object)DateTime.Parse(scheduleCalendarExclusionDTO.ExclusionDate)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@includeDate", (scheduleCalendarExclusionDTO.IncludeDate == true ? "Y" : "N")));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@day", scheduleCalendarExclusionDTO.Day == -1 ? DBNull.Value : (object)scheduleCalendarExclusionDTO.Day));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", (scheduleCalendarExclusionDTO.IsActive == true ? "Y" : "N")));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", scheduleCalendarExclusionDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the schedule exclusion record to the database
        /// </summary>
        /// <param name="scheduleCalendarExclusion">ScheduleExclusionDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public ScheduleCalendarExclusionDTO InsertScheduleCalendarExclusion(ScheduleCalendarExclusionDTO scheduleCalendarExclusion, string loginId, int siteId)
        {
            log.LogMethodEntry(scheduleCalendarExclusion, loginId, siteId);
            string insertScheduleExclusionQuery = @"INSERT INTO [dbo].[Schedule_ExclusionDays]  
                                                        ( 
                                                          ScheduleId,
                                                          ExclusionDate,
                                                          IncludeDate,
                                                          Day,
                                                          IsActive,
                                                          CreatedBy,
                                                          CreationDate,
                                                          LastUpdatedBy,
                                                          LastupdatedDate,
                                                          Guid,
                                                          site_id,  
                                                          MasterEntityId
                                                        ) 
                                                values 
                                                        (                                                           
                                                          @scheduleId,
                                                          @exclusionDate,
                                                          @includeDate,
                                                          @day,
                                                          @isActive,
                                                          @createdBy,
                                                          getdate(),
                                                          @lastUpdatedBy,
                                                          getdate(),
                                                          NEWID(),
                                                          @siteid, 
                                                          @masterEntityId
                                                        )SELECT * FROM Schedule_ExclusionDays WHERE ScheduleExclusionId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertScheduleExclusionQuery, GetSQLParameters(scheduleCalendarExclusion, loginId, siteId).ToArray(), sqlTransaction);
                RefreshScheduleCalendarExclusionDTO(scheduleCalendarExclusion, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting scheduleCalendarExclusionDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(scheduleCalendarExclusion);
            return scheduleCalendarExclusion;
        }


        /// <summary>
        /// Updates the schedule exclusion record
        /// </summary>
        /// <param name="scheduleCalendarExclusion">ScheduleExclusionDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public ScheduleCalendarExclusionDTO UpdateScheduleCalendarExclusion(ScheduleCalendarExclusionDTO scheduleCalendarExclusion, string loginId, int siteId)
        {
            log.LogMethodEntry(scheduleCalendarExclusion, loginId, siteId);
            string updateScheduleExclusionQuery = @"update Schedule_ExclusionDays 
                                         set ScheduleId=@scheduleId,
                                                          ExclusionDate=@exclusionDate,
                                                          IncludeDate=@includeDate,
                                                          Day=@day,
                                                          IsActive=@isActive,                                                          
                                                          LastUpdatedBy=@lastUpdatedBy,
                                                          LastupdatedDate=GetDate(),                                                          
                                                          --site_id=@siteid, 
                                                          MasterEntityId=@masterEntityId
                                       where ScheduleExclusionId = @scheduleExclusionId
                                       SELECT* FROM Schedule_ExclusionDays WHERE  ScheduleExclusionId = @scheduleExclusionId ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateScheduleExclusionQuery, GetSQLParameters(scheduleCalendarExclusion, loginId, siteId).ToArray(), sqlTransaction);
                RefreshScheduleCalendarExclusionDTO(scheduleCalendarExclusion, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating scheduleCalendarExclusionDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(scheduleCalendarExclusion);
            return scheduleCalendarExclusion;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="scheduleCalendarExclusionDTO">scheduleCalendarExclusionDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshScheduleCalendarExclusionDTO(ScheduleCalendarExclusionDTO scheduleCalendarExclusionDTO, DataTable dt)
        {
            log.LogMethodEntry(scheduleCalendarExclusionDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                scheduleCalendarExclusionDTO.ScheduleExclusionId = Convert.ToInt32(dt.Rows[0]["ScheduleExclusionId"]);
                scheduleCalendarExclusionDTO.LastupdateDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                scheduleCalendarExclusionDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                scheduleCalendarExclusionDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                scheduleCalendarExclusionDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                scheduleCalendarExclusionDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                scheduleCalendarExclusionDTO.Siteid = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to ScheduleCalendarExclusionDTO class type
        /// </summary>
        /// <param name="scheduleCalendarExclusionDataRow">ScheduleCalendarExclusionDTO DataRow</param>
        /// <returns>Returns ScheduleCalendarExclusionDTO</returns>
        private ScheduleCalendarExclusionDTO GetScheduleCalendarExclusionDTO(DataRow scheduleCalendarExclusionDataRow)
        {
            log.LogMethodEntry(scheduleCalendarExclusionDataRow);
            ScheduleCalendarExclusionDTO scheduleDataObject = new ScheduleCalendarExclusionDTO(Convert.ToInt32(scheduleCalendarExclusionDataRow["ScheduleExclusionId"]),
                                            Convert.ToInt32(scheduleCalendarExclusionDataRow["ScheduleId"]),
                                            scheduleCalendarExclusionDataRow["ExclusionDate"] .ToString(),
                                            scheduleCalendarExclusionDataRow["IncludeDate"] == DBNull.Value ? false : (scheduleCalendarExclusionDataRow["IncludeDate"].ToString() == "Y" ? true : false), 
                                            scheduleCalendarExclusionDataRow["Day"] == DBNull.Value ? -1 : Convert.ToInt32(scheduleCalendarExclusionDataRow["Day"]),
                                            scheduleCalendarExclusionDataRow["IsActive"] == DBNull.Value ? true : (scheduleCalendarExclusionDataRow["IsActive"].ToString() == "Y"? true: false), 
                                            scheduleCalendarExclusionDataRow["CreatedBy"].ToString(),
                                            scheduleCalendarExclusionDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(scheduleCalendarExclusionDataRow["CreationDate"]),
                                            scheduleCalendarExclusionDataRow["LastUpdatedBy"].ToString(),
                                            scheduleCalendarExclusionDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(scheduleCalendarExclusionDataRow["LastupdatedDate"]),
                                            scheduleCalendarExclusionDataRow["Guid"].ToString(),
                                            scheduleCalendarExclusionDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(scheduleCalendarExclusionDataRow["site_id"]),
                                            scheduleCalendarExclusionDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(scheduleCalendarExclusionDataRow["SynchStatus"]),
                                            scheduleCalendarExclusionDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(scheduleCalendarExclusionDataRow["MasterEntityId"])
                                            );
            log.LogMethodExit(scheduleDataObject);
            return scheduleDataObject;
        }

        /// <summary>
        /// Gets the schedule Calendar exclusion data of passed schedule Calendar exclusion Id
        /// </summary>
        /// <param name="scheduleCalendarExclusionId">integer type parameter</param>
        /// <returns>Returns ScheduleCalendarExclusionDTO</returns>
        public ScheduleCalendarExclusionDTO GetScheduleCalendarExclusion(int scheduleCalendarExclusionId)
        {
            log.LogMethodEntry(scheduleCalendarExclusionId);
            string selectScheduleQuery = SELECT_QUERY + @" WHERE se.ScheduleExclusionId = @scheduleExclusionId";
            SqlParameter[] selectScheduleExclusionParameters = new SqlParameter[1];
            selectScheduleExclusionParameters[0] = new SqlParameter("@scheduleExclusionId", scheduleCalendarExclusionId);
            DataTable schedule = dataAccessHandler.executeSelectQuery(selectScheduleQuery, selectScheduleExclusionParameters, sqlTransaction);
            ScheduleCalendarExclusionDTO scheduleExclusionDataObject = null;
            if (schedule.Rows.Count > 0)
            {
                DataRow scheduleExclusionRow = schedule.Rows[0];
                scheduleExclusionDataObject = GetScheduleCalendarExclusionDTO(scheduleExclusionRow); 
            }
            log.LogMethodExit(scheduleExclusionDataObject);
            return scheduleExclusionDataObject;
        }

        /// <summary>
        /// Gets the ScheduleCalendarExclusionDTO List for schedule Id List
        /// </summary>
        /// <param name="scheduleIdList">integer list parameter</param>
        /// <returns>Returns List of ScheduleCalendarExclusionDTO</returns>
        public List<ScheduleCalendarExclusionDTO> GetScheduleCalendarExclusionDTOList(List<int> scheduleIdList, bool activeRecords)
        {
            log.LogMethodEntry(scheduleIdList);
            List<ScheduleCalendarExclusionDTO> list = new List<ScheduleCalendarExclusionDTO>();
            string query = @"SELECT Schedule_ExclusionDays.*
                            FROM Schedule_ExclusionDays, @scheduleIdList List
                            WHERE ScheduleId = List.Id ";
            if (activeRecords)
            {
                query += " AND IsActive = 'Y' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@scheduleIdList", scheduleIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetScheduleCalendarExclusionDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// helps to find the existence inclusion exclusion record for the passed parameter.
        /// </summary>
        /// <param name="scheduleID">scheduleID</param>
        /// <param name="dtDate">dtDate</param>
        /// <returns>bool type value</returns>
        public bool GetExclusionDays(int scheduleID, DateTime dtDate)
        {
            log.LogMethodEntry(scheduleID, dtDate);
            string sqlQuery = @"select 1 from Schedule_ExclusionDays 
                                                                    where ScheduleId = @id 
                                                                    and (ExclusionDate = @date or Day = DATEPART(WEEKDAY, @Date)) 
                                                                    and IncludeDate = 'N'";
            SqlParameter[] selectScheduleExclusionParameters = new SqlParameter[2];
            selectScheduleExclusionParameters[0] = new SqlParameter("@id", scheduleID);
            selectScheduleExclusionParameters[1] = new SqlParameter("@Date", dtDate);
            DataTable dTable = dataAccessHandler.executeSelectQuery(sqlQuery, selectScheduleExclusionParameters, sqlTransaction);
            if (dTable == null || dTable.Rows.Count == 0)
                return false;
            log.LogMethodExit();
            return true;
        }

        /// <summary>
        /// Gets the ScheduleCalendarExclusionDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ScheduleCalendarExclusionDTO matching the search criteria</returns>
        public List<ScheduleCalendarExclusionDTO> GetScheduleCalendarExclusionList(List<KeyValuePair<ScheduleCalendarExclusionDTO.SearchByScheduleCalendarExclusionParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<SqlParameter> parameters = new List<SqlParameter>();
            int count = 0;
            string selectScheduleExclusionQuery = SELECT_QUERY;
            if (searchParameters != null)
            {
                string joiner = " ";
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<ScheduleCalendarExclusionDTO.SearchByScheduleCalendarExclusionParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count > 0) ? " and " : string.Empty;
                        if (searchParameter.Key == ScheduleCalendarExclusionDTO.SearchByScheduleCalendarExclusionParameters.SCHEDULE_ID
                            || searchParameter.Key == ScheduleCalendarExclusionDTO.SearchByScheduleCalendarExclusionParameters.MASTER_ENTITY_ID 
                            || searchParameter.Key == ScheduleCalendarExclusionDTO.SearchByScheduleCalendarExclusionParameters.SCHEDULE_EXCLUSION_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ScheduleCalendarExclusionDTO.SearchByScheduleCalendarExclusionParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ScheduleCalendarExclusionDTO.SearchByScheduleCalendarExclusionParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
                        }
                        else if (searchParameter.Key == ScheduleCalendarExclusionDTO.SearchByScheduleCalendarExclusionParameters.EXCLUSION_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), SiteContainerList.ToSiteDateTime(executionContext, DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture))));
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
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                }
                if (searchParameters.Count > 0)
                    selectScheduleExclusionQuery = selectScheduleExclusionQuery + query;
            }
            DataTable ScheduleExclusionData = dataAccessHandler.executeSelectQuery(selectScheduleExclusionQuery, parameters.ToArray(), sqlTransaction);
            List<ScheduleCalendarExclusionDTO> scheduleCalendarExclusionList = null;
            if (ScheduleExclusionData.Rows.Count > 0)
            {
                scheduleCalendarExclusionList = new List<ScheduleCalendarExclusionDTO>();
                foreach (DataRow ScheduleExclusionDataRow in ScheduleExclusionData.Rows)
                {
                    ScheduleCalendarExclusionDTO scheduleExclusionDataObject = GetScheduleCalendarExclusionDTO(ScheduleExclusionDataRow);
                    scheduleCalendarExclusionList.Add(scheduleExclusionDataObject);
                }
            }
            log.LogMethodExit(scheduleCalendarExclusionList);
            return scheduleCalendarExclusionList;
        }
    }
}
