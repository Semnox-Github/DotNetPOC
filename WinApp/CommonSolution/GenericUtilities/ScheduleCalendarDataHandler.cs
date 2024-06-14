/********************************************************************************************
 * Project Name - ScheduleCalendarDataHandler 
 * Description  - Data handler of the Schedule class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        30-Dec-2015   Raghuveera          Created 
 *********************************************************************************************
 *1.00        18-Jul-2016   Raghuveera          Modified 
 *********************************************************************************************
 *1.00        24-Apr-2017   Lakshminarayana     Modified 
 *2.70        08-Mar-2019   Guru S A            Renamed ScheduleDataHandler as ScheduleCalendarDataHandler
 *2.70        07-Jun-2019   Akshay Gulaganji    Added SCHEDULE_ID_LIST searchParam in GetScheduleCalendarDTOList()
 *2.70.2        26-Jul-2019   Dakshakh Raj        Modified : added GetSQLParameters(), 
 *                                                          SQL injection Issue Fix.
 *2.70.2       06-Dec-2019   Jinto Thomas            Removed siteid from update query                                                           
  *2.90         07-Aug-2020   Mushahid Faizan     Modified : default isActive value to true.
 *2.100.0     31-Aug-2020   Mushahid Faizan   siteId changes in GetSQLParameters().
 *2.150.0      04-May-2021   Abhishek         Modified : fetch data using Idlist  
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
    /// Schedule Data Handler - Handles insert, update and select of schedule data objects
    /// </summary>
    public class ScheduleCalendarDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction = null;
        private const string SELECT_QUERY = @"SELECT * FROM Schedule as ss ";
        private ExecutionContext executionContext;

        /// <summary>
        /// Dictionary for searching Parameters for the ScheduleCalendar object.
        /// </summary>
        private static readonly Dictionary<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string> DBSearchParameters = new Dictionary<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>
            {
                {ScheduleCalendarDTO.SearchByScheduleCalendarParameters.SCHEDULE_ID, "ss.ScheduleId"},
                {ScheduleCalendarDTO.SearchByScheduleCalendarParameters.SCHEDULE_NAME, "ss.ScheduleName"},
                {ScheduleCalendarDTO.SearchByScheduleCalendarParameters.RECUR_END_DATE, "ss.RecurEndDate"},
                {ScheduleCalendarDTO.SearchByScheduleCalendarParameters.SCHEDULE_TIME, "ss.ScheduleTime"},
                {ScheduleCalendarDTO.SearchByScheduleCalendarParameters.IS_ACTIVE, "ss.IsActive"},
                {ScheduleCalendarDTO.SearchByScheduleCalendarParameters.MASTER_ENTITY_ID, "ss.MasterEntityId"},
                {ScheduleCalendarDTO.SearchByScheduleCalendarParameters.SITE_ID, "ss.site_id"},
                {ScheduleCalendarDTO.SearchByScheduleCalendarParameters.SCHEDULE_ID_LIST, "ss.ScheduleId"},
                {ScheduleCalendarDTO.SearchByScheduleCalendarParameters.SCHEDULE_FROM_TIME, "ss.ScheduleTime"},
                {ScheduleCalendarDTO.SearchByScheduleCalendarParameters.SCHEDULE_TO_TIME, "ss.ScheduleTime"}
            };

        /// <summary>
        /// Default constructor of ScheduleDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ScheduleCalendarDataHandler(ExecutionContext executionContext,SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            this.executionContext = executionContext;
            this.dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        ///Builds the SQL Parameter list used for inserting and updating scheduleDTO Reecord.
        /// </summary>
        /// <param name="scheduleCalendarDTO">scheduleCalendarDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns> Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(ScheduleCalendarDTO scheduleCalendarDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(scheduleCalendarDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@scheduleId", scheduleCalendarDTO.ScheduleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@scheduleName", scheduleCalendarDTO.ScheduleName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@scheduleTime", scheduleCalendarDTO.ScheduleTime == DateTime.MinValue ? DBNull.Value : (object)scheduleCalendarDTO.ScheduleTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@scheduleEndDate", scheduleCalendarDTO.ScheduleEndDate == DateTime.MinValue ? DBNull.Value : (object)scheduleCalendarDTO.ScheduleEndDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@recurFlag", scheduleCalendarDTO.RecurFlag));
            parameters.Add(dataAccessHandler.GetSQLParameter("@recurFrequency", string.IsNullOrEmpty(scheduleCalendarDTO.RecurFrequency) ? DBNull.Value : (object)scheduleCalendarDTO.RecurFrequency));
            parameters.Add(dataAccessHandler.GetSQLParameter("@recurEndDate", scheduleCalendarDTO.RecurEndDate == DateTime.MinValue ? DBNull.Value : (object)scheduleCalendarDTO.RecurEndDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@recurType", string.IsNullOrEmpty(scheduleCalendarDTO.RecurType) ? DBNull.Value : (object)scheduleCalendarDTO.RecurType));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", (scheduleCalendarDTO.IsActive == true ? "Y" : "N")));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", scheduleCalendarDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@synchStatus", scheduleCalendarDTO.SynchStatus));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the schedule record to the database
        /// </summary>
        /// <param name="scheduleCalendarDTO">ScheduleCalendarDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public ScheduleCalendarDTO Insert(ScheduleCalendarDTO scheduleCalendarDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(scheduleCalendarDTO, loginId, siteId);
            string insertScheduleQuery = @"insert into Schedule 
                                                        (    
                                                          ScheduleName,
                                                          ScheduleTime,
                                                          ScheduleEndDate,
                                                          recurFlag,
                                                          RecurFrequency,
                                                          RecurEndDate,
                                                          RecurType,
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
                                                          @scheduleName,
                                                          @scheduleTime,
                                                          @scheduleEndDate,
                                                          @recurFlag,
                                                          @recurFrequency,
                                                          @recurEndDate,
                                                          @recurType,
                                                          @isActive,
                                                          @createdBy,
                                                          Getdate(), 
                                                          @lastUpdatedBy,
                                                          GetDate(),                                                       
                                                          Newid(),
                                                          @siteid, 
                                                          @masterEntityId
                                                        )SELECT * FROM Schedule WHERE ScheduleId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertScheduleQuery, GetSQLParameters(scheduleCalendarDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshScheduleCalendarDTO(scheduleCalendarDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting scheduleCalendarDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(scheduleCalendarDTO);
            return scheduleCalendarDTO;
        }

        /// <summary>
        /// Updates the ScheduleCalendar record
        /// </summary>
        /// <param name="scheduleCalendarDTO">ScheduleCalendarDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public ScheduleCalendarDTO Update(ScheduleCalendarDTO scheduleCalendarDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(scheduleCalendarDTO, loginId, siteId);
            string updateScheduleQuery = @"update Schedule 
                                         set ScheduleName=@scheduleName,
                                             ScheduleTime=@scheduleTime,
                                             ScheduleEndDate=@scheduleEndDate,
                                             recurFlag=@recurFlag,
                                             RecurFrequency=@recurFrequency,
                                             RecurEndDate=@recurEndDate,
                                             RecurType=@recurType,
                                             IsActive = @isActive,
                                             LastUpdatedBy = @lastUpdatedBy, 
                                             LastupdatedDate = Getdate(),
                                             --site_id=@siteid, 
                                             MasterEntityId=@masterEntityId
                                       where ScheduleId = @scheduleId
                                       SELECT* FROM Schedule WHERE ScheduleId = @scheduleId ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateScheduleQuery, GetSQLParameters(scheduleCalendarDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshScheduleCalendarDTO(scheduleCalendarDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating scheduleCalendarDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(scheduleCalendarDTO);
            return scheduleCalendarDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="scheduleCalendarDTO">scheduleCalendarDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshScheduleCalendarDTO(ScheduleCalendarDTO scheduleCalendarDTO, DataTable dt)
        {
            log.LogMethodEntry(scheduleCalendarDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                scheduleCalendarDTO.ScheduleId = Convert.ToInt32(dt.Rows[0]["ScheduleId"]);
                scheduleCalendarDTO.LastupdateDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                scheduleCalendarDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                scheduleCalendarDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                scheduleCalendarDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                scheduleCalendarDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                scheduleCalendarDTO.Siteid = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to ScheduleCalendarDTO class type
        /// </summary>
        /// <param name="scheduleCalendarDataRow">ScheduleCalendarDTO DataRow</param>
        /// <returns>Returns ScheduleCalendarDTO</returns>
        private ScheduleCalendarDTO GetScheduleCalendarDTO(DataRow scheduleCalendarDataRow)
        {
            log.LogMethodEntry(scheduleCalendarDataRow);
            ScheduleCalendarDTO scheduleCalendarDataObject = new ScheduleCalendarDTO(Convert.ToInt32(scheduleCalendarDataRow["ScheduleId"]),
                                            scheduleCalendarDataRow["ScheduleName"].ToString(),
                                            scheduleCalendarDataRow["ScheduleTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(scheduleCalendarDataRow["ScheduleTime"]),
                                            scheduleCalendarDataRow["ScheduleEndDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(scheduleCalendarDataRow["ScheduleEndDate"]),
                                            scheduleCalendarDataRow["recurFlag"].ToString(),
                                            scheduleCalendarDataRow["RecurFrequency"].ToString(),
                                            scheduleCalendarDataRow["RecurEndDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(scheduleCalendarDataRow["RecurEndDate"]),
                                            scheduleCalendarDataRow["RecurType"].ToString(),
                                            scheduleCalendarDataRow["IsActive"] == DBNull.Value ? true : (scheduleCalendarDataRow["IsActive"].ToString() == "Y" ? true : false),
                                            scheduleCalendarDataRow["CreatedBy"].ToString(),
                                            scheduleCalendarDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(scheduleCalendarDataRow["CreationDate"]),
                                            scheduleCalendarDataRow["LastUpdatedBy"].ToString(),
                                            scheduleCalendarDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(scheduleCalendarDataRow["LastupdatedDate"]),
                                            scheduleCalendarDataRow["Guid"].ToString(),
                                            scheduleCalendarDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(scheduleCalendarDataRow["site_id"]),
                                            scheduleCalendarDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(scheduleCalendarDataRow["SynchStatus"]),
                                            scheduleCalendarDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(scheduleCalendarDataRow["MasterEntityId"])
                                            );
            log.LogMethodExit(scheduleCalendarDataObject);
            return scheduleCalendarDataObject;
        }

        internal List<ScheduleCalendarDTO> GetScheduleCalendarDTOList(List<int> scheduleIdList, bool activeRecords) //added
        {
            log.LogMethodEntry(scheduleIdList);
            List<ScheduleCalendarDTO> scheduleCalendarDTOList = new List<ScheduleCalendarDTO>();
            string query = @"SELECT *
                            FROM Schedule, @scheduleIdList List
                            WHERE ScheduleId = List.Id ";
            if (activeRecords)
            {
                query += " AND isActive = 'Y' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@scheduleIdList", scheduleIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                scheduleCalendarDTOList = table.Rows.Cast<DataRow>().Select(x => GetScheduleCalendarDTO(x)).ToList();
            }
            log.LogMethodExit(scheduleCalendarDTOList);
            return scheduleCalendarDTOList;
        }
        /// <summary>
        /// Gets the schedule data of passed schedule calendar Id
        /// </summary>
        /// <param name="scheduleCalendarId">integer type parameter</param>
        /// <returns>Returns ScheduleCalendarDTO</returns>
        public ScheduleCalendarDTO GetScheduleCalendarDTO(int scheduleCalendarId)
        {
            log.LogMethodEntry(scheduleCalendarId);
            string selectScheduleQuery = SELECT_QUERY + @" WHERE ss.ScheduleId = @scheduleId";
            SqlParameter[] selectScheduleParameters = new SqlParameter[1];
            selectScheduleParameters[0] = new SqlParameter("@scheduleId", scheduleCalendarId);
            DataTable schedule = dataAccessHandler.executeSelectQuery(selectScheduleQuery, selectScheduleParameters, sqlTransaction);
            ScheduleCalendarDTO scheduleCalendarDataObject = null;
            if (schedule.Rows.Count > 0)
            {
                DataRow scheduleRow = schedule.Rows[0];
                scheduleCalendarDataObject = GetScheduleCalendarDTO(scheduleRow);
            }
            log.LogMethodExit(scheduleCalendarDataObject);
            return scheduleCalendarDataObject;
        }

        /// <summary>
        /// Gets the day and week schedule records
        /// </summary>
        /// <param name="dtFromDate">From date </param>
        /// <param name="dtToDate">To date</param>
        /// <param name="siteId"> Site Id</param>
        /// <returns>ScheduleDTO list</returns>
        public List<ScheduleCalendarDTO> GetScheduleDayWeekList(DateTime dtFromDate, DateTime dtToDate, int siteId)
        {
            log.LogMethodEntry(dtFromDate, dtToDate, siteId);
            string selectScheduleQuery = @"Select * " +
                                "from Schedule " +
                                "where (site_id = @siteid or @siteid = -1) " +
                                "and ((ScheduleTime >= @fromDate and ScheduleTime < @toDate)" +
                                "or (recurFlag = 'Y' and ((RecurEndDate >= @fromdate and ScheduleTime < @todate)))) " +
                                "order by ScheduleTime desc";
            SqlParameter[] selectScheduleParameters = new SqlParameter[3];
            selectScheduleParameters[0] = new SqlParameter("@fromDate", dtFromDate.Date);
            selectScheduleParameters[1] = new SqlParameter("@toDate", dtToDate.Date);
            selectScheduleParameters[2] = new SqlParameter("@siteid", siteId);

            DataTable schedule = dataAccessHandler.executeSelectQuery(selectScheduleQuery, selectScheduleParameters, sqlTransaction);
            List<ScheduleCalendarDTO> scheduleCalendarDTOList = null;
            if (schedule.Rows.Count > 0)
            {
                scheduleCalendarDTOList = new List<ScheduleCalendarDTO>();
                foreach (DataRow scheduleDataRow in schedule.Rows)
                {
                    ScheduleCalendarDTO scheduleDataObject = GetScheduleCalendarDTO(scheduleDataRow);
                    scheduleCalendarDTOList.Add(scheduleDataObject);
                }
            }
            log.LogMethodExit(scheduleCalendarDTOList);
            return scheduleCalendarDTOList;
        }

        /// <summary>
        /// Gets the ScheduleCalendarDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ScheduleCalendarDTO matching the search criteria</returns>
        public List<ScheduleCalendarDTO> GetScheduleCalendarDTOList(List<KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            List<ScheduleCalendarDTO> scheduleList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            int count = 0;
            string selectScheduleQuery = SELECT_QUERY;
            if (searchParameters != null)
            {
                string joiner = " ";//starts:Modification on 18-Jul-2016 for publish feature
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count > 0) ? " and " : string.Empty;
                        if (searchParameter.Key == ScheduleCalendarDTO.SearchByScheduleCalendarParameters.SCHEDULE_ID
                            || searchParameter.Key == ScheduleCalendarDTO.SearchByScheduleCalendarParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ScheduleCalendarDTO.SearchByScheduleCalendarParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ScheduleCalendarDTO.SearchByScheduleCalendarParameters.RECUR_END_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), SiteContainerList.ToSiteDateTime(executionContext, DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture))));
                        }
                        else if (searchParameter.Key == ScheduleCalendarDTO.SearchByScheduleCalendarParameters.SCHEDULE_TIME)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), SiteContainerList.ToSiteDateTime(executionContext, DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture))));
                        }
                        else if (searchParameter.Key == ScheduleCalendarDTO.SearchByScheduleCalendarParameters.SCHEDULE_FROM_TIME)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), SiteContainerList.ToSiteDateTime(executionContext, DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture))));
                        }
                        else if (searchParameter.Key == ScheduleCalendarDTO.SearchByScheduleCalendarParameters.SCHEDULE_TO_TIME)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) < " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), SiteContainerList.ToSiteDateTime(executionContext, DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture))));
                        }
                        else if (searchParameter.Key == ScheduleCalendarDTO.SearchByScheduleCalendarParameters.SCHEDULE_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == ScheduleCalendarDTO.SearchByScheduleCalendarParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        count++;//Ends:Modification on 18-Jul-2016 for publish feature
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
                    selectScheduleQuery = selectScheduleQuery + query;
            }
            DataTable scheduleData = dataAccessHandler.executeSelectQuery(selectScheduleQuery, parameters.ToArray(), sqlTransaction);
            if (scheduleData.Rows.Count > 0)
            {
                scheduleList = new List<ScheduleCalendarDTO>();
                foreach (DataRow scheduleDataRow in scheduleData.Rows)
                {
                    ScheduleCalendarDTO scheduleCalendarDTO = GetScheduleCalendarDTO(scheduleDataRow);
                    scheduleList.Add(scheduleCalendarDTO);
                }

            }
            log.LogMethodExit(scheduleList);
            return scheduleList;
        }

        /// <summary>
        /// Gets the ScheduleCalendarDTO List for Schedule Id List
        /// </summary>
        /// <param name="scheduleIdList">integer list parameter</param>
        /// <returns>Returns List of ScheduleCalendarDTO</returns>
        public List<ScheduleCalendarDTO> GetScheduleDTOListOfSchedules(List<int> scheduleIdList, bool activeRecords)
        {
            log.LogMethodEntry(scheduleIdList);
            List<ScheduleCalendarDTO> list = new List<ScheduleCalendarDTO>();
            string query = SELECT_QUERY + @",@ScheduleIdList List  WHERE ss.ScheduleId = List.Id"; 
            if (activeRecords)
            {
                query += " AND IsActive = 'Y' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@ScheduleIdList", scheduleIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetScheduleCalendarDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }
    }
}
