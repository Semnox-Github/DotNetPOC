/********************************************************************************************
 * Project Name - Schedule Data Handler
 * Description  - Data handler of the Schedule class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        30-Dec-2015   Raghuveera          Created 
 *********************************************************************************************
 *1.00        18-Jul-2016   Raghuveera          Modified 
 *********************************************************************************************
 *1.00        24-Apr-2017   Lakshminarayana     Modified 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Semnox.Core.Utilities;
namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Schedule Data Handler - Handles insert, update and select of schedule data objects
    /// </summary>
    public class ScheduleDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<ScheduleDTO.SearchByScheduleParameters, string> DBSearchParameters = new Dictionary<ScheduleDTO.SearchByScheduleParameters, string>
            {
                {ScheduleDTO.SearchByScheduleParameters.SCHEDULE_ID, "ScheduleId"},
                {ScheduleDTO.SearchByScheduleParameters.SCHEDULE_NAME, "ScheduleName"},
                {ScheduleDTO.SearchByScheduleParameters.RECUR_END_DATE, "RecurEndDate"},
                {ScheduleDTO.SearchByScheduleParameters.SCHEDULE_TIME, "ScheduleTime"},
                {ScheduleDTO.SearchByScheduleParameters.IS_ACTIVE, "IsActive"},
                {ScheduleDTO.SearchByScheduleParameters.MASTER_ENTITY_ID, "MasterEntityId"},//Starts:Modification on 18-Jul-2016 for publish feature
                {ScheduleDTO.SearchByScheduleParameters.SITE_ID, "site_id"}//Ends: Modification on 18-Jul-2016 for publish feature
            };
        DataAccessHandler dataAccessHandler;

        /// <summary>
        /// Default constructor of ScheduleDataHandler class
        /// </summary>
        public ScheduleDataHandler()
        {
            log.Debug("Starts-ScheduleDataHandler() default constructor.");
            dataAccessHandler = new DataAccessHandler();
            log.Debug("Ends-ScheduleDataHandler() default constructor.");
        }

        /// <summary>
        /// Inserts the schedule record to the database
        /// </summary>
        /// <param name="schedule">ScheduleDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertSchedule(ScheduleDTO schedule, string userId, int siteId)
        {
            log.Debug("Starts-InsertSchedule(schedule, userId, siteId) Method.");
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
                                                          SynchStatus,
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
                                                          @synchStatus,
                                                          @masterEntityId
                                                        )SELECT CAST(scope_identity() AS int)";
            List<SqlParameter> updateScheduleParameters = new List<SqlParameter>();
            updateScheduleParameters.Add(new SqlParameter("@scheduleName", schedule.ScheduleName));
            if (schedule.ScheduleTime.Date.Equals(DateTime.MinValue))
            {
                updateScheduleParameters.Add(new SqlParameter("@scheduleTime", DBNull.Value));
            }
            else
            {
                updateScheduleParameters.Add(new SqlParameter("@scheduleTime", schedule.ScheduleTime));
            }
            if (schedule.ScheduleEndDate.Date.Equals(DateTime.MinValue))
            {
                updateScheduleParameters.Add(new SqlParameter("@scheduleEndDate", DBNull.Value));
            }
            else
            {
                updateScheduleParameters.Add(new SqlParameter("@scheduleEndDate", schedule.ScheduleEndDate));
            }
            updateScheduleParameters.Add(new SqlParameter("@recurFlag", schedule.RecurFlag));
            if (string.IsNullOrEmpty(schedule.RecurFrequency))
            {
                updateScheduleParameters.Add(new SqlParameter("@recurFrequency", DBNull.Value));
            }
            else
            {
                updateScheduleParameters.Add(new SqlParameter("@recurFrequency", schedule.RecurFrequency));
            }

            if (schedule.RecurEndDate.Date.Equals(DateTime.MinValue))
            {
                updateScheduleParameters.Add(new SqlParameter("@recurEndDate", DBNull.Value));
            }
            else
            {
                updateScheduleParameters.Add(new SqlParameter("@recurEndDate", schedule.RecurEndDate));
            }
            if (string.IsNullOrEmpty(schedule.RecurType))
            {
                updateScheduleParameters.Add(new SqlParameter("@recurType", DBNull.Value));
            }
            else
            {
                updateScheduleParameters.Add(new SqlParameter("@recurType", schedule.RecurType));
            }
            updateScheduleParameters.Add(new SqlParameter("@isActive", (schedule.IsActive == true? "Y":"N")));
            updateScheduleParameters.Add(new SqlParameter("@createdBy", userId));
            updateScheduleParameters.Add(new SqlParameter("@lastUpdatedBy", userId));
            if (siteId == -1)
                updateScheduleParameters.Add(new SqlParameter("@siteId", DBNull.Value));
            else
                updateScheduleParameters.Add(new SqlParameter("@siteId", siteId));
            if (schedule.SynchStatus)
            {
                updateScheduleParameters.Add(new SqlParameter("@synchStatus", schedule.SynchStatus));
            }
            else
            {
                updateScheduleParameters.Add(new SqlParameter("@synchStatus", DBNull.Value));
            }
            if (schedule.MasterEntityId == -1)//starts:Modification on 18-Jul-2016 for publish feature
            {
                updateScheduleParameters.Add(new SqlParameter("@masterEntityId", DBNull.Value));
            }
            else
            {
                updateScheduleParameters.Add(new SqlParameter("@masterEntityId", schedule.MasterEntityId));
            }//Ends:Modification on 18-Jul-2016 for publish feature
            int idOfRowInserted = dataAccessHandler.executeInsertQuery(insertScheduleQuery, updateScheduleParameters.ToArray());
            log.Debug("Ends-InsertSchedule(schedule, userId, siteId) Method.");
            return idOfRowInserted;
        }

        /// <summary>
        /// Updates the schedule record
        /// </summary>
        /// <param name="schedule">ScheduleDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public int UpdateSchedule(ScheduleDTO schedule, string userId, int siteId)
        {
            log.Debug("Starts-UpdateSchedule(schedule, userId, siteId) Method.");
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
                                             SynchStatus = @synchStatus,
                                             MasterEntityId=@masterEntityId
                                       where ScheduleId = @scheduleId";
            List<SqlParameter> updateScheduleParameters = new List<SqlParameter>();
            updateScheduleParameters.Add(new SqlParameter("@scheduleId", schedule.ScheduleId));
            updateScheduleParameters.Add(new SqlParameter("@scheduleName", schedule.ScheduleName));
            if (schedule.ScheduleTime.Date.Equals(DateTime.MinValue))
            {
                updateScheduleParameters.Add(new SqlParameter("@scheduleTime", DBNull.Value));
            }
            else
            {
                updateScheduleParameters.Add(new SqlParameter("@scheduleTime", schedule.ScheduleTime));
            }
            if (schedule.ScheduleEndDate.Date.Equals(DateTime.MinValue))
            {
                updateScheduleParameters.Add(new SqlParameter("@scheduleEndDate", DBNull.Value));
            }
            else
            {
                updateScheduleParameters.Add(new SqlParameter("@scheduleEndDate", schedule.ScheduleEndDate));
            }
            updateScheduleParameters.Add(new SqlParameter("@recurFlag", schedule.RecurFlag));
            if (string.IsNullOrEmpty(schedule.RecurFrequency))
            {
                updateScheduleParameters.Add(new SqlParameter("@recurFrequency", DBNull.Value));
            }
            else
            {
                updateScheduleParameters.Add(new SqlParameter("@recurFrequency", schedule.RecurFrequency));
            }

            if (schedule.RecurEndDate.Date.Equals(DateTime.MinValue))
            {
                updateScheduleParameters.Add(new SqlParameter("@recurEndDate", DBNull.Value));
            }
            else
            {
                updateScheduleParameters.Add(new SqlParameter("@recurEndDate", schedule.RecurEndDate));
            }
            if (string.IsNullOrEmpty(schedule.RecurType))
            {
                updateScheduleParameters.Add(new SqlParameter("@recurType", DBNull.Value));
            }
            else
            {
                updateScheduleParameters.Add(new SqlParameter("@recurType", schedule.RecurType));
            }
            updateScheduleParameters.Add(new SqlParameter("@isActive", (schedule.IsActive == true ? "Y" : "N")));
            updateScheduleParameters.Add(new SqlParameter("@lastUpdatedBy", userId));
            if (siteId == -1)
                updateScheduleParameters.Add(new SqlParameter("@siteId", DBNull.Value));
            else
                updateScheduleParameters.Add(new SqlParameter("@siteId", siteId));
            if (schedule.SynchStatus)
            {
                updateScheduleParameters.Add(new SqlParameter("@synchStatus", schedule.SynchStatus));
            }
            else
            {
                updateScheduleParameters.Add(new SqlParameter("@synchStatus", DBNull.Value));
            }
            if (schedule.MasterEntityId == -1)//starts:Modification on 18-Jul-2016 for publish feature
            {
                updateScheduleParameters.Add(new SqlParameter("@masterEntityId", DBNull.Value));
            }
            else
            {
                updateScheduleParameters.Add(new SqlParameter("@masterEntityId", schedule.MasterEntityId));
            }//Ends:Modification on 18-Jul-2016 for publish feature
            int rowsUpdated = dataAccessHandler.executeUpdateQuery(updateScheduleQuery, updateScheduleParameters.ToArray());
            log.Debug("Ends-UpdateSchedule(schedule, userId, siteId) Method.");
            return rowsUpdated;
        }

        /// <summary>
        /// Converts the Data row object to ScheduleDTO class type
        /// </summary>
        /// <param name="scheduleDataRow">ScheduleDTO DataRow</param>
        /// <returns>Returns ScheduleDTO</returns>
        private ScheduleDTO GetScheduleDTO(DataRow scheduleDataRow)
        {
            log.Debug("Starts-GetScheduleDTO(scheduleDataRow) Method.");
            ScheduleDTO scheduleDataObject = new ScheduleDTO(Convert.ToInt32(scheduleDataRow["ScheduleId"]),
                                            scheduleDataRow["ScheduleName"].ToString(),
                                            scheduleDataRow["ScheduleTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(scheduleDataRow["ScheduleTime"]),
                                            scheduleDataRow["ScheduleEndDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(scheduleDataRow["ScheduleEndDate"]),
                                            scheduleDataRow["recurFlag"].ToString(),
                                            scheduleDataRow["RecurFrequency"].ToString(),
                                            scheduleDataRow["RecurEndDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(scheduleDataRow["RecurEndDate"]),
                                            scheduleDataRow["RecurType"].ToString(),
                                            scheduleDataRow["IsActive"] == DBNull.Value ? false : (scheduleDataRow["IsActive"].ToString() == "Y"? true: false), 
                                            scheduleDataRow["CreatedBy"].ToString(),
                                            scheduleDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(scheduleDataRow["CreationDate"]),
                                            scheduleDataRow["LastUpdatedBy"].ToString(),
                                            scheduleDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(scheduleDataRow["LastupdatedDate"]),
                                            scheduleDataRow["Guid"].ToString(),
                                            scheduleDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(scheduleDataRow["site_id"]),
                                            scheduleDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(scheduleDataRow["SynchStatus"]),
                                            scheduleDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(scheduleDataRow["MasterEntityId"])//Modification on 18-Jul-2016 for publish feature
                                            );
            log.Debug("Ends-GetScheduleDTO(scheduleDataRow) Method.");
            return scheduleDataObject;
        }

        /// <summary>
        /// Gets the schedule data of passed schedule Id
        /// </summary>
        /// <param name="scheduleId">integer type parameter</param>
        /// <returns>Returns ScheduleDTO</returns>
        public ScheduleDTO GetSchedule(int scheduleId)
        {
            log.Debug("Starts-GetSchedule(scheduleId) Method.");
            string selectScheduleQuery = @"select *
                                         from Schedule
                                        where ScheduleId = @scheduleId";
            SqlParameter[] selectScheduleParameters = new SqlParameter[1];
            selectScheduleParameters[0] = new SqlParameter("@scheduleId", scheduleId);
            DataTable schedule = dataAccessHandler.executeSelectQuery(selectScheduleQuery, selectScheduleParameters);
            if (schedule.Rows.Count > 0)
            {
                DataRow scheduleRow = schedule.Rows[0];
                ScheduleDTO scheduleDataObject = GetScheduleDTO(scheduleRow);
                log.Debug("Ends-GetSchedule(scheduleId) Method by returnting scheduleDataObject.");
                return scheduleDataObject;
            }
            else
            {
                log.Debug("Ends-GetSchedule(scheduleId) Method by returnting null.");
                return null;
            }
        }

        /// <summary>
        /// Gets the day and week schedule records
        /// </summary>
        /// <param name="dtFromDate">From date </param>
        /// <param name="dtToDate">To date</param>
        /// <param name="siteId"> Site Id</param>
        /// <returns>ScheduleDTO list</returns>
        public List<ScheduleDTO> GetScheduleDayWeekList(DateTime dtFromDate, DateTime dtToDate, int siteId)
        {
            log.Debug("Starts-GetSchedule(dtFromDate,siteId) Method.");
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



            DataTable schedule = dataAccessHandler.executeSelectQuery(selectScheduleQuery, selectScheduleParameters);
            if (schedule.Rows.Count > 0)
            {
                List<ScheduleDTO> scheduleList = new List<ScheduleDTO>();
                foreach (DataRow scheduleDataRow in schedule.Rows)
                {
                    ScheduleDTO scheduleDataObject = GetScheduleDTO(scheduleDataRow);
                    scheduleList.Add(scheduleDataObject);
                }
                log.Debug("Ends-GetSchedule(dtFromDate,siteId) Method by returning scheduleList.");
                return scheduleList;
            }
            else
            {
                log.Debug("Ends-GetSchedule(dtFromDate,siteId) Method by returning null.");
                return null;
            }
        }

        /// <summary>
        /// Gets the ScheduleDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ScheduleDTO matching the search criteria</returns>
        public List<ScheduleDTO> GetScheduleList(List<KeyValuePair<ScheduleDTO.SearchByScheduleParameters, string>> searchParameters)
        {
            log.Debug("Starts-GetScheduleList(searchParameters) Method.");
            int count = 0;
            string selectScheduleQuery = @"select *
                                         from Schedule";
            if (searchParameters != null)
            {
                string joiner = " ";//starts:Modification on 18-Jul-2016 for publish feature
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<ScheduleDTO.SearchByScheduleParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count > 0) ? " and " : " ";
                        if (searchParameter.Key == ScheduleDTO.SearchByScheduleParameters.SCHEDULE_ID || searchParameter.Key == ScheduleDTO.SearchByScheduleParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value);
                        }
                        else if (searchParameter.Key == ScheduleDTO.SearchByScheduleParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                        }
                        else if (searchParameter.Key == ScheduleDTO.SearchByScheduleParameters.RECUR_END_DATE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + ">= " + "'" + searchParameter.Value + "'");
                        }
                        else if (searchParameter.Key == ScheduleDTO.SearchByScheduleParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'N') = " + (searchParameter.Value.ToString() == "1"? "'Y'":"'N'" ));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%" + searchParameter.Value + "%'");
                        }
                        count++;//Ends:Modification on 18-Jul-2016 for publish feature
                    }
                    else
                    {
                        log.Debug("Ends-GetScheduleList(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                if (searchParameters.Count > 0)
                    selectScheduleQuery = selectScheduleQuery + query;
            }
            DataTable scheduleData = dataAccessHandler.executeSelectQuery(selectScheduleQuery, null);
            if (scheduleData.Rows.Count > 0)
            {
                List<ScheduleDTO> scheduleList = new List<ScheduleDTO>();
                foreach (DataRow scheduleDataRow in scheduleData.Rows)
                {
                    ScheduleDTO scheduleDataObject = GetScheduleDTO(scheduleDataRow);
                    scheduleList.Add(scheduleDataObject);
                }
                log.Debug("Ends-GetScheduleList(searchParameters) Method by returning scheduleList.");
                return scheduleList;
            }
            else
            {
                log.Debug("Ends-GetScheduleList(searchParameters) Method by returning null.");
                return null;
            }
        }
    }
}
