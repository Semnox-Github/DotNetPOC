/********************************************************************************************
 * Project Name - Maintenance Schedule Data Handler
 * Description  - Data handler of the maintenance schedule class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        30-Dec-2015   Raghuveera          Created 
 *********************************************************************************************
 *1.00        18-Jul-2016   Raghuveera          Modified 
 *2.70        08-Mar-2019   Guru S A            Renamed MaintenanceScheduleDataHandler as JobScheduleDataHandler
 *2.70.2        25-Jul-2019   Dakshakh Raj        Modified : added GetSQLParameters(), 
 *                                                          SQL injection Issue Fix and
 *2.70.2        06-Dec-2019   Jinto Thomas            Removed siteid from update query                                                           
*2.90         11-Aug-2020   Mushahid Faizan     Modified : Added GetJobScheduleDTOList() and changed default isActive value to true.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Maintenance Schedule Data Handler - Handles insert, update and select of maintenance schedule data objects
    /// </summary>
    public class JobScheduleDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string SELECT_QUERY = @"SELECT * FROM Maint_Schedule as ms ";

        /// <summary>
        /// Dictionary for searching Parameters for the JobScheduleDTO object.
        /// </summary>
        private static readonly Dictionary<JobScheduleDTO.SearchByJobScheduleDTOParameters, string> DBSearchParameters = new Dictionary<JobScheduleDTO.SearchByJobScheduleDTOParameters, string>
            {
                {JobScheduleDTO.SearchByJobScheduleDTOParameters.SCHEDULE_ID, "ms.ScheduleId"},
                {JobScheduleDTO.SearchByJobScheduleDTOParameters.JOB_SCHEDULE_ID, "ms.MaintScheduleId"},
                {JobScheduleDTO.SearchByJobScheduleDTOParameters.IS_ACTIVE, "ms.IsActive"},
                {JobScheduleDTO.SearchByJobScheduleDTOParameters.MASTER_ENTITY_ID, "ms.MasterEntityId"}, 
                {JobScheduleDTO.SearchByJobScheduleDTOParameters.SITE_ID, "ms.site_id"} 
            };
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTrx;

        /// <summary>
        /// Default constructor of JobScheduleDataHandler class
        /// </summary>
        public JobScheduleDataHandler(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            this.dataAccessHandler = new DataAccessHandler();
            this.sqlTrx = sqlTrx;
            log.LogMethodExit();
        }

        /// <summary>
        ///Builds the SQL Parameter list used for inserting and updating JobScheduleDataHandler Reecord.
        /// </summary>
        /// <param name="jobScheduleDTO">jobScheduleDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns> Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(JobScheduleDTO jobScheduleDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(jobScheduleDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@JobScheduleId", jobScheduleDTO.JobScheduleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@scheduleId", jobScheduleDTO.ScheduleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@userId", jobScheduleDTO.UserId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@departmentId", jobScheduleDTO.DepartmentId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@durationToComplete", (jobScheduleDTO.DurationToComplete == -1 ? DBNull.Value : (object)jobScheduleDTO.DurationToComplete)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@maxValueJobCreated", jobScheduleDTO.MaxValueJobCreated == DateTime.MinValue ? DBNull.Value : (object)jobScheduleDTO.MaxValueJobCreated));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", (jobScheduleDTO.IsActive == true ? "Y" : "N")));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", jobScheduleDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the JObSchedule record to the database
        /// </summary>
        /// <param name="jobScheduleDTO">jobScheduleDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns jobScheduleDTO</returns>
        public JobScheduleDTO InsertJobSchedule(JobScheduleDTO jobScheduleDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(jobScheduleDTO, loginId, siteId);
            string insertMaintenanceScheduleQuery = @"insert into Maint_Schedule 
                                                        (
                                                          ScheduleId,
                                                          UserId,
                                                          DepartmentId,
                                                          DurationToComplete,
                                                          MaxValueJobCreated,
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
                                                          @userId,
                                                          @departmentId,
                                                          @durationToComplete,
                                                          @maxValueJobCreated,                                                          
                                                          @isActive,
                                                          @createdBy,
                                                          Getdate(),
                                                          @lastUpdatedBy,
                                                          Getdate(),                                                        
                                                          Newid(),
                                                          @siteid, 
                                                          @masterEntityId
                                                        )SELECT * FROM Maint_Schedule WHERE MaintScheduleId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertMaintenanceScheduleQuery, GetSQLParameters(jobScheduleDTO, loginId, siteId).ToArray(), sqlTrx);
                RefreshJobScheduleDTO(jobScheduleDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting jobScheduleDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(jobScheduleDTO);
            return jobScheduleDTO;
        }

        /// <summary>
        /// Updates the JOb schedule record
        /// </summary>
        /// <param name="jobScheduleDTO">jobScheduleDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public JobScheduleDTO UpdateJobSchedule(JobScheduleDTO jobScheduleDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(jobScheduleDTO, loginId, siteId);
            string updateMaintenanceScheduleQuery = @"update Maint_Schedule 
                                         set ScheduleId=@scheduleId,
                                             UserId=@userId,
                                             DepartmentId=@departmentId,
                                             DurationToComplete=@durationToComplete,
                                             MaxValueJobCreated=@maxValueJobCreated,
                                             IsActive = @isActive,
                                             LastUpdatedBy = @lastUpdatedBy, 
                                             LastupdatedDate = Getdate(),
                                             --site_id=@siteid, 
                                             MasterEntityId=@masterEntityId
                                       where MaintScheduleId = @JobScheduleId
                                       SELECT* FROM Maint_Schedule WHERE  MaintScheduleId = @JobScheduleId ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateMaintenanceScheduleQuery, GetSQLParameters(jobScheduleDTO, loginId, siteId).ToArray(), sqlTrx);
                RefreshJobScheduleDTO(jobScheduleDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating jobScheduleDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(jobScheduleDTO);
            return jobScheduleDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="jobScheduleDTO">jobScheduleDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshJobScheduleDTO(JobScheduleDTO jobScheduleDTO, DataTable dt)
        {
            log.LogMethodEntry(jobScheduleDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                jobScheduleDTO.JobScheduleId = Convert.ToInt32(dt.Rows[0]["MaintScheduleId"]);
                jobScheduleDTO.LastupdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                jobScheduleDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                jobScheduleDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                jobScheduleDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                jobScheduleDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                jobScheduleDTO.Siteid = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to JobScheduleDTO class type
        /// </summary>
        /// <param name="jobScheduleDataRow">JobScheduleDTO DataRow</param>
        /// <returns>Returns JobScheduleDTO</returns>
        private JobScheduleDTO GetJobScheduleDTO(DataRow jobScheduleDataRow)
        {
            log.LogMethodEntry(jobScheduleDataRow);
            JobScheduleDTO jobScheduleDataObject = new JobScheduleDTO(Convert.ToInt32(jobScheduleDataRow["MaintScheduleId"]),
                                            Convert.ToInt32(jobScheduleDataRow["ScheduleId"]),
                                            Convert.ToInt32(jobScheduleDataRow["UserId"]),
                                            jobScheduleDataRow["DepartmentId"] == DBNull.Value ? -1 : Convert.ToInt32(jobScheduleDataRow["DepartmentId"]),
                                            jobScheduleDataRow["DurationToComplete"] == DBNull.Value ? -1 : Convert.ToInt32(jobScheduleDataRow["DurationToComplete"]),
                                            jobScheduleDataRow["MaxValueJobCreated"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(jobScheduleDataRow["MaxValueJobCreated"]),
                                            jobScheduleDataRow["IsActive"] == DBNull.Value ? true : (jobScheduleDataRow["IsActive"].ToString() == "Y" ? true : false),
                                            jobScheduleDataRow["CreatedBy"].ToString(),
                                            jobScheduleDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(jobScheduleDataRow["CreationDate"]),
                                            jobScheduleDataRow["LastUpdatedBy"].ToString(),
                                            jobScheduleDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(jobScheduleDataRow["LastupdatedDate"]),
                                            jobScheduleDataRow["Guid"].ToString(),
                                            jobScheduleDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(jobScheduleDataRow["site_id"]),
                                            jobScheduleDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(jobScheduleDataRow["SynchStatus"]),
                                            jobScheduleDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(jobScheduleDataRow["MasterEntityId"])//Modification on 18-Jul-2016 for publish feature
                                            );
            log.LogMethodExit(jobScheduleDataObject);
            return jobScheduleDataObject;
        }

        /// <summary>
        /// Gets the Job schedule data of passed schedule Id
        /// </summary>
        /// <param name="jobScheduleId">integer type parameter</param>
        /// <returns>Returns JobScheduleDTO</returns>
        public JobScheduleDTO GetJobScheduleDTO(int jobScheduleId)
        {
            log.LogMethodEntry(jobScheduleId);
            string selectMaintenanceScheduleQuery = SELECT_QUERY + @" WHERE ms.MaintScheduleId = @maintScheduleId";
            SqlParameter[] selectMaintenanceScheduleParameters = new SqlParameter[1];
            selectMaintenanceScheduleParameters[0] = new SqlParameter("@maintScheduleId", jobScheduleId);
            DataTable maintenanceSchedule = dataAccessHandler.executeSelectQuery(selectMaintenanceScheduleQuery, selectMaintenanceScheduleParameters, sqlTrx);
            JobScheduleDTO jobScheduleDataObject = null;
            if (maintenanceSchedule.Rows.Count > 0)
            {
                DataRow maintenanceScheduleRow = maintenanceSchedule.Rows[0];
                jobScheduleDataObject = GetJobScheduleDTO(maintenanceScheduleRow);
            }
            log.LogMethodExit(jobScheduleDataObject);
            return jobScheduleDataObject;
        }

        /// <summary>
        /// Gets the JobScheduleDTO List for schedule Id List
        /// </summary>
        /// <param name="scheduleIdList">integer list parameter</param>
        /// <returns>Returns List of DSignageLookupValuesDTOList</returns>
        public List<JobScheduleDTO> GetJobScheduleDTOList(List<int> scheduleIdList, bool activeRecords)
        {
            log.LogMethodEntry(scheduleIdList);
            List<JobScheduleDTO> list = new List<JobScheduleDTO>();
            string query = @"SELECT Maint_Schedule.*
                            FROM Maint_Schedule, @scheduleIdList List
                            WHERE ScheduleId = List.Id ";
            if (activeRecords)
            {
                query += " AND IsActive = 'Y' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@scheduleIdList", scheduleIdList, null, sqlTrx);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetJobScheduleDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// Gets the JobScheduleDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of JobScheduleDTO matching the search criteria</returns>
        public List<JobScheduleDTO> GetJobScheduleDTOList(List<KeyValuePair<JobScheduleDTO.SearchByJobScheduleDTOParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectMaintenanceScheduleQuery = SELECT_QUERY;
            if (searchParameters != null)
            {
                string joiner = " ";//starts:Modification on 18-Jul-2016 for publish feature
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<JobScheduleDTO.SearchByJobScheduleDTOParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count > 0) ? " and " : string.Empty;
                        if (searchParameter.Key == JobScheduleDTO.SearchByJobScheduleDTOParameters.SCHEDULE_ID 
                            || searchParameter.Key == JobScheduleDTO.SearchByJobScheduleDTOParameters.MASTER_ENTITY_ID
                            || searchParameter.Key == JobScheduleDTO.SearchByJobScheduleDTOParameters.JOB_SCHEDULE_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == JobScheduleDTO.SearchByJobScheduleDTOParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == JobScheduleDTO.SearchByJobScheduleDTOParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
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

                if ((searchParameters != null) && (searchParameters.Count > 0))
                    selectMaintenanceScheduleQuery = selectMaintenanceScheduleQuery + query;
            }
            DataTable maintenanceScheduleData = dataAccessHandler.executeSelectQuery(selectMaintenanceScheduleQuery, parameters.ToArray(), sqlTrx);
            List<JobScheduleDTO> jobScheduleDTOList = null;
            if (maintenanceScheduleData.Rows.Count > 0)
            {
                jobScheduleDTOList = new List<JobScheduleDTO>();
                foreach (DataRow maintenanceScheduleDataRow in maintenanceScheduleData.Rows)
                {
                    JobScheduleDTO maintenanceScheduleDataObject = GetJobScheduleDTO(maintenanceScheduleDataRow);
                    jobScheduleDTOList.Add(maintenanceScheduleDataObject);
                } 
            }
            log.LogMethodExit(jobScheduleDTOList);
            return jobScheduleDTOList;
        }
    }
}
