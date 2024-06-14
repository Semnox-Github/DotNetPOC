/********************************************************************************************
 * Project Name - Job Scheduler Data Handler
 * Description  - Data handler of the job scheduler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        05-Feb-2016   Raghuveera     Created 
 *2.70       11-Mar-2019   Guru S A       Rename JobScheduleDataHandler as JobSchedulerDataHandler
 *2.70.2        10-Dec-2019   Jinto Thomas     Removed siteid from update query                                                          
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// Job Scheduler Data Handler
    /// </summary>
    public class JobSchedulerDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DataAccessHandler dataAccessHandler;
        /// <summary>
        /// Default constructor of JobScheduleDataHandler class
        /// </summary>
        public JobSchedulerDataHandler()
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }
        /// <summary>
        /// Inserts the job Scheduler record to the database
        /// </summary>
        /// <param name="jobSchedulerDTO">jobSchedulerDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertJobScheduler(JobSchedulerDTO jobSchedulerDTO, string userId, int siteId)
        {
            log.LogMethodEntry(jobSchedulerDTO, userId, siteId);
            string insertJobScheduleQuery = @"insert into Maint_JobSchedule
                                                        ( 
                                                         JobName,
                                                         LastSuccessfulRunTime,
                                                         IsActive,
                                                         Guid,
                                                         CreatedBy,
                                                         CreationDate,
                                                         LastUpdatedBy,
                                                         LastUpdatedDate,
                                                         site_id )
                                                values 
                                                        (
                                                         @jobName,
                                                         @lastSuccessfulRunTime,
                                                         @isActive,
                                                         Newid(),
                                                         @createdBy,
                                                         Getdate(), 
                                                         @lastUpdatedBy,
                                                         GetDate(),
                                                         @siteId 
                                                        )SELECT CAST(scope_identity() AS int)";
            List<SqlParameter> updateJobScheduleParameters = new List<SqlParameter>();
            if (string.IsNullOrEmpty(jobSchedulerDTO.JobName))
            {
                updateJobScheduleParameters.Add(new SqlParameter("@jobName", DBNull.Value));
            }
            else
            {
                updateJobScheduleParameters.Add(new SqlParameter("@jobName", jobSchedulerDTO.JobName));
            }            
            if (jobSchedulerDTO.LastSuccessfulRunTime.Equals(DateTime.MinValue))
            {
                updateJobScheduleParameters.Add(new SqlParameter("@lastSuccessfulRunTime", DBNull.Value));
            }
            else
            {
                updateJobScheduleParameters.Add(new SqlParameter("@lastSuccessfulRunTime", jobSchedulerDTO.LastSuccessfulRunTime));
            }
            updateJobScheduleParameters.Add(new SqlParameter("@isActive", (jobSchedulerDTO.IsActive == true? "Y":"N")));
            updateJobScheduleParameters.Add(new SqlParameter("@createdBy", userId));
            updateJobScheduleParameters.Add(new SqlParameter("@lastUpdatedBy", userId));
            if (siteId == -1)
                updateJobScheduleParameters.Add(new SqlParameter("@siteId", DBNull.Value));
            else
                updateJobScheduleParameters.Add(new SqlParameter("@siteId", siteId)); 
            int idOfRowInserted = dataAccessHandler.executeInsertQuery(insertJobScheduleQuery, updateJobScheduleParameters.ToArray());
            log.LogMethodExit(idOfRowInserted);
            return idOfRowInserted;
        }
        /// <summary>
        /// Updates the job Scheduler record
        /// </summary>
        /// <param name="jobSchedulerDTO">jobSchedulerDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public int UpdateJobScheduler(JobSchedulerDTO jobSchedulerDTO, string userId, int siteId)
        {
            log.LogMethodEntry(jobSchedulerDTO, userId, siteId);
            string updateJobScheduleQuery = @"update Maint_JobSchedule
                                         set JobName=@jobName,
                                             LastSuccessfulRunTime=@lastSuccessfulRunTime,
                                             IsActive=@isActive,
                                             LastUpdatedBy=@lastUpdatedBy,
                                             LastUpdatedDate=getDate()
                                             --site_id=@siteId 
                                       where JobId = @jobId";
            List<SqlParameter> updateJobScheduleParameters = new List<SqlParameter>();
            updateJobScheduleParameters.Add(new SqlParameter("@jobId", jobSchedulerDTO.JobId));
            if (string.IsNullOrEmpty(jobSchedulerDTO.JobName))
            {
                updateJobScheduleParameters.Add(new SqlParameter("@jobName", DBNull.Value));
            }
            else
            {
                updateJobScheduleParameters.Add(new SqlParameter("@jobName", jobSchedulerDTO.JobName));
            }
            if (jobSchedulerDTO.LastSuccessfulRunTime.Equals(DateTime.MinValue))
            {
                updateJobScheduleParameters.Add(new SqlParameter("@lastSuccessfulRunTime", DBNull.Value));
            }
            else
            {
                updateJobScheduleParameters.Add(new SqlParameter("@lastSuccessfulRunTime", jobSchedulerDTO.LastSuccessfulRunTime));
            }
            updateJobScheduleParameters.Add(new SqlParameter("@isActive", (jobSchedulerDTO.IsActive == true ? "Y" : "N"))); 
            updateJobScheduleParameters.Add(new SqlParameter("@lastUpdatedBy", userId));
            if (siteId == -1)
                updateJobScheduleParameters.Add(new SqlParameter("@siteId", DBNull.Value));
            else
                updateJobScheduleParameters.Add(new SqlParameter("@siteId", siteId)); 
            int rowsUpdated = dataAccessHandler.executeUpdateQuery(updateJobScheduleQuery, updateJobScheduleParameters.ToArray());
            log.LogMethodExit(rowsUpdated);
            return rowsUpdated;
        }
        /// <summary>
        /// Converts the Data row object to JobSchedulerDTO class type
        /// </summary>
        /// <param name="jobSchedulerDataRow">Job Scheduler DataRow</param>
        /// <returns>Returns JobScheduler</returns>
        private JobSchedulerDTO GetJobSchedulerDTO(DataRow jobSchedulerDataRow)
        {
            log.LogMethodEntry(jobSchedulerDataRow);
            JobSchedulerDTO jobSchedulerDataObject = new JobSchedulerDTO(Convert.ToInt32(jobSchedulerDataRow["JobId"]),
                                            jobSchedulerDataRow["JobName"].ToString(),
                                            jobSchedulerDataRow["LastSuccessfulRunTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(jobSchedulerDataRow["LastSuccessfulRunTime"]),
                                            jobSchedulerDataRow["IsActive"] == DBNull.Value ? false :(jobSchedulerDataRow["IsActive"].ToString() == "Y"? true: false), 
                                            jobSchedulerDataRow["CreatedBy"].ToString(),
                                            jobSchedulerDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(jobSchedulerDataRow["CreationDate"]),
                                            jobSchedulerDataRow["LastUpdatedBy"].ToString(),
                                            jobSchedulerDataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(jobSchedulerDataRow["LastUpdatedDate"]),
                                            jobSchedulerDataRow["Guid"].ToString(),
                                            jobSchedulerDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(jobSchedulerDataRow["site_id"]),
                                            jobSchedulerDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(jobSchedulerDataRow["SynchStatus"])
                                            );
            log.LogMethodExit(jobSchedulerDataObject);
            return jobSchedulerDataObject;
        }
        /// <summary>
        /// Gets the job scheduler data of passed job Id
        /// </summary>
        /// <param name="jobSchedulerId">integer type parameter</param>
        /// <returns>Returns JobScheduleDTO</returns>
        public JobSchedulerDTO GetJobSchedulerDTO(int jobSchedulerId)
        {
            log.LogMethodEntry(jobSchedulerId);
            string selectJobScheduleQuery = @"select *
                                         from Maint_JobSchedule
                                        where JobId = @jobId";
            SqlParameter[] selectJobScheduleParameters = new SqlParameter[1];
            selectJobScheduleParameters[0] = new SqlParameter("@jobId", jobSchedulerId);
            DataTable jobSchedule = dataAccessHandler.executeSelectQuery(selectJobScheduleQuery, selectJobScheduleParameters);
            JobSchedulerDTO jobSchedulerDataObject = null;
            if (jobSchedule.Rows.Count > 0)
            {
                DataRow jobScheduleRow = jobSchedule.Rows[0];
                jobSchedulerDataObject = GetJobSchedulerDTO(jobScheduleRow); 
            }
            log.LogMethodExit(jobSchedulerDataObject);
            return jobSchedulerDataObject;
        }
        /// <summary>
        /// Gets the JobSchedulerDTO list matching the search key
        /// </summary>
        /// <returns>Returns the list of JobSchedulerDTO matching the search criteria</returns>
        public List<JobSchedulerDTO> GetJobSchedulerDTOList()
        {
            log.LogMethodEntry();
            string selectJobScheduleQuery = @"SELECT *
                                                 FROM Maint_JobSchedule";
            DataTable jobScheduleData = dataAccessHandler.executeSelectQuery(selectJobScheduleQuery, null);
            List<JobSchedulerDTO> jobSchedulerDTOList = null; 
            if (jobScheduleData.Rows.Count > 0)
            {
                jobSchedulerDTOList = new List<JobSchedulerDTO>();
                foreach (DataRow jobScheduleDataRow in jobScheduleData.Rows)
                {
                    JobSchedulerDTO jobScheduleDataObject = GetJobSchedulerDTO(jobScheduleDataRow);
                    jobSchedulerDTOList.Add(jobScheduleDataObject);
                } 
            }
            log.LogMethodExit(jobSchedulerDTOList);
            return jobSchedulerDTOList;
        }
    }
}
