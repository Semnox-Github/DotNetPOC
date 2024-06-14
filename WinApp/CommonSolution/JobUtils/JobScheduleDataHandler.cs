/********************************************************************************************
 * Project Name - Job Schedule Data Handler
 * Description  - Data handler of the job schedule class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        05-Feb-2016   Raghuveera          Created 
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
    /// Job Schedule Data Handler
    /// </summary>
    public class JobScheduleDataHandler
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DataAccessHandler dataAccessHandler;
        /// <summary>
        /// Default constructor of JobScheduleDataHandler class
        /// </summary>
        public JobScheduleDataHandler()
        {
            log.Debug("Starts-JobScheduleDataHandler() default constructor.");
            dataAccessHandler = new DataAccessHandler();
            log.Debug("Ends-JobScheduleDataHandler() default constructor.");
        }
        /// <summary>
        /// Inserts the job Schedule record to the database
        /// </summary>
        /// <param name="jobSchedule">JobScheduleDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertJobSchedule(JobScheduleDTO jobSchedule, string userId, int siteId)
        {
            log.Debug("Starts-InsertJobSchedule(jobSchedule, userId, siteId) Method.");
            string insertJobScheduleQuery = @"insert into Maint_JobSchedule
                                                        ( 
                                                         JobName,
                                                         LastSuccessfulRunTime,
                                                         IsActive,
                                                         Guid,
                                                         CreatedBy,
                                                         CreationDate,
                                                         UpdatedBy,
                                                         UpdatedDate,
                                                         site_id,
                                                         SynchStatus 
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
                                                         @siteId,
                                                         @synchStatus
                                                        )SELECT CAST(scope_identity() AS int)";
            List<SqlParameter> updateJobScheduleParameters = new List<SqlParameter>();
            if (string.IsNullOrEmpty(jobSchedule.JobName))
            {
                updateJobScheduleParameters.Add(new SqlParameter("@jobName", DBNull.Value));
            }
            else
            {
                updateJobScheduleParameters.Add(new SqlParameter("@jobName", jobSchedule.JobName));
            }            
            if (jobSchedule.LastSuccessfulRunTime.Equals(DateTime.MinValue))
            {
                updateJobScheduleParameters.Add(new SqlParameter("@lastSuccessfulRunTime", DBNull.Value));
            }
            else
            {
                updateJobScheduleParameters.Add(new SqlParameter("@lastSuccessfulRunTime", jobSchedule.LastSuccessfulRunTime));
            }
            updateJobScheduleParameters.Add(new SqlParameter("@isActive", jobSchedule.IsActive));
            updateJobScheduleParameters.Add(new SqlParameter("@createdBy", userId));
            updateJobScheduleParameters.Add(new SqlParameter("@lastUpdatedBy", userId));
            if (siteId == -1)
                updateJobScheduleParameters.Add(new SqlParameter("@siteId", DBNull.Value));
            else
                updateJobScheduleParameters.Add(new SqlParameter("@siteId", siteId));
            updateJobScheduleParameters.Add(new SqlParameter("@synchStatus", jobSchedule.SynchStatus));
            int idOfRowInserted = dataAccessHandler.executeInsertQuery(insertJobScheduleQuery, updateJobScheduleParameters.ToArray());
            log.Debug("Ends-InsertJobSchedule(jobSchedule, userId, siteId) Method.");
            return idOfRowInserted;
        }
        /// <summary>
        /// Updates the job Schedule record
        /// </summary>
        /// <param name="jobSchedule">JobScheduleDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public int UpdateJobSchedule(JobScheduleDTO jobSchedule, string userId, int siteId)
        {
            log.Debug("Starts-UpdateJobSchedule(jobSchedule, userId, siteId) Method.");
            string updateJobScheduleQuery = @"update Maint_JobSchedule
                                         set JobName=@jobName,
                                             LastSuccessfulRunTime=@lastSuccessfulRunTime,
                                             IsActive=@isActive,
                                             LastUpdatedBy=@lastUpdatedBy,
                                             LastUpdatedDate=getDate(),
                                             -- site_id=@siteId,
                                             SynchStatus=@synchStatus
                                       where JobId = @jobId";
            List<SqlParameter> updateJobScheduleParameters = new List<SqlParameter>();
            updateJobScheduleParameters.Add(new SqlParameter("@jobId", jobSchedule.JobId));
            if (string.IsNullOrEmpty(jobSchedule.JobName))
            {
                updateJobScheduleParameters.Add(new SqlParameter("@jobName", DBNull.Value));
            }
            else
            {
                updateJobScheduleParameters.Add(new SqlParameter("@jobName", jobSchedule.JobName));
            }
            if (jobSchedule.LastSuccessfulRunTime.Equals(DateTime.MinValue))
            {
                updateJobScheduleParameters.Add(new SqlParameter("@lastSuccessfulRunTime", DBNull.Value));
            }
            else
            {
                updateJobScheduleParameters.Add(new SqlParameter("@lastSuccessfulRunTime", jobSchedule.LastSuccessfulRunTime));
            }
            updateJobScheduleParameters.Add(new SqlParameter("@isActive", jobSchedule.IsActive));
            updateJobScheduleParameters.Add(new SqlParameter("@createdBy", userId));
            updateJobScheduleParameters.Add(new SqlParameter("@lastUpdatedBy", userId));
            if (siteId == -1)
                updateJobScheduleParameters.Add(new SqlParameter("@siteId", DBNull.Value));
            else
                updateJobScheduleParameters.Add(new SqlParameter("@siteId", siteId));
            updateJobScheduleParameters.Add(new SqlParameter("@synchStatus", jobSchedule.SynchStatus));
            int rowsUpdated = dataAccessHandler.executeUpdateQuery(updateJobScheduleQuery, updateJobScheduleParameters.ToArray());
            log.Debug("Ends-UpdateJobSchedule(jobSchedule, userId, siteId) Method.");
            return rowsUpdated;
        }
        /// <summary>
        /// Converts the Data row object to JobScheduleDTO class type
        /// </summary>
        /// <param name="jobScheduleDataRow">Job Schedule DataRow</param>
        /// <returns>Returns JobSchedule</returns>
        private JobScheduleDTO GetJobScheduleDTO(DataRow jobScheduleDataRow)
        {
            log.Debug("Starts-GetJobScheduleDTO(jobScheduleDataRow) Method.");
            JobScheduleDTO jobScheduleDataObject = new JobScheduleDTO(Convert.ToInt32(jobScheduleDataRow["JobId"]),
                                            jobScheduleDataRow["JobName"].ToString(),
                                            jobScheduleDataRow["LastSuccessfulRunTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(jobScheduleDataRow["LastSuccessfulRunTime"]),
                                            jobScheduleDataRow["IsActive"].ToString(),
                                            jobScheduleDataRow["CreatedBy"].ToString(),
                                            jobScheduleDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(jobScheduleDataRow["CreationDate"]),
                                            jobScheduleDataRow["LastUpdatedBy"].ToString(),
                                            jobScheduleDataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(jobScheduleDataRow["LastUpdatedDate"]),
                                            jobScheduleDataRow["Guid"].ToString(),
                                            jobScheduleDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(jobScheduleDataRow["site_id"]),
                                            jobScheduleDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(jobScheduleDataRow["SynchStatus"])
                                            );
            log.Debug("Ends-GetJobScheduleDTO(jobScheduleDataRow) Method.");
            return jobScheduleDataObject;
        }
        /// <summary>
        /// Gets the job schedule data of passed job Id
        /// </summary>
        /// <param name="jobScheduleId">integer type parameter</param>
        /// <returns>Returns JobScheduleDTO</returns>
        public JobScheduleDTO GetJobSchedule(int jobScheduleId)
        {
            log.Debug("Starts-GetJobSchedule(jobScheduleId) Method.");
            string selectJobScheduleQuery = @"select *
                                         from Maint_JobSchedule
                                        where JobId = @jobId";
            SqlParameter[] selectJobScheduleParameters = new SqlParameter[1];
            selectJobScheduleParameters[0] = new SqlParameter("@jobId", jobScheduleId);
            DataTable jobSchedule = dataAccessHandler.executeSelectQuery(selectJobScheduleQuery, selectJobScheduleParameters);
            if (jobSchedule.Rows.Count > 0)
            {
                DataRow jobScheduleRow = jobSchedule.Rows[0];
                JobScheduleDTO jobScheduleDataObject = GetJobScheduleDTO(jobScheduleRow);
                log.Debug("Ends-GetJobSchedule(jobScheduleId) Method by returnting jobScheduleDataObject.");
                return jobScheduleDataObject;
            }
            else
            {
                log.Debug("Ends-GetJobSchedule(jobScheduleId) Method by returnting null.");
                return null;
            }
        }
        /// <summary>
        /// Gets the JobScheduleDTO list matching the search key
        /// </summary>
        /// <returns>Returns the list of JobScheduleDTO matching the search criteria</returns>
        public List<JobScheduleDTO> GetJobScheduleList()
        {
            log.Debug("Starts-GetJobScheduleList() Method.");
            string selectJobScheduleQuery = @"SELECT *
                                                 FROM Maint_JobSchedule";
            DataTable jobScheduleData = dataAccessHandler.executeSelectQuery(selectJobScheduleQuery, null);
            if (jobScheduleData.Rows.Count > 0)
            {
                List<JobScheduleDTO> jobScheduleList = new List<JobScheduleDTO>();
                foreach (DataRow jobScheduleDataRow in jobScheduleData.Rows)
                {
                    JobScheduleDTO jobScheduleDataObject = GetJobScheduleDTO(jobScheduleDataRow);
                    jobScheduleList.Add(jobScheduleDataObject);
                }
                log.Debug("Ends-GetJobScheduleList() Method by returning jobScheduleList.");
                return jobScheduleList;
            }
            else
            {
                log.Debug("Ends-GetJobScheduleList() Method by returning null.");
                return null;
            }
        }
    }
}
