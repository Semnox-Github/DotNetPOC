/********************************************************************************************
 * Project Name - Job Task Data Handler
 * Description  - Data handler of the Job task class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        23-Dec-2015   Raghuveera     Created 
 *********************************************************************************************
 *1.00        18-Jul-2016   Raghuveera     Modified 
 *2.70        08-Mar-2019   Guru S A       Renamed MaintenanceTaskDataHandler as JobTaskDatahandler
 *2.70        07-Jul-2019   Dakshakh raj   Modified (Added SELECT_QUERY,GetSQLParameters, SqlInjection issue Fix)
 *2.70.2        10-Dec-2019   Jinto Thomas     Removed siteid from update query                                                          
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Maintenance
{
    /// <summary>
    /// Job Task Data Handler - Handles insert, update and select of job Task Data objects
    /// </summary>
    public class JobTaskDatahandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM Maint_Tasks ";

        /// <summary>
        /// Dictionary for searching Parameters for the job Task object.
        /// </summary>
        private static readonly Dictionary<JobTaskDTO.SearchByJobTaskParameters, string> DBSearchParameters = new Dictionary<JobTaskDTO.SearchByJobTaskParameters, string>
            {                
                {JobTaskDTO.SearchByJobTaskParameters.TASK_NAME, "TaskName"},
                {JobTaskDTO.SearchByJobTaskParameters.JOB_TASK_GROUP_ID, "MaintTaskGroupId"},
                {JobTaskDTO.SearchByJobTaskParameters.IS_ACTIVE, "IsActive"},
                {JobTaskDTO.SearchByJobTaskParameters.MASTER_ENTITY_ID,"MasterEntityId"},
                {JobTaskDTO.SearchByJobTaskParameters.SITE_ID, "site_id"},
                {JobTaskDTO.SearchByJobTaskParameters.JOB_TASK_ID, "MaintTaskId"}
            };

        /// <summary>
        /// Default constructor of JobTaskDatahandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public JobTaskDatahandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry();
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating jobTaskDTO Record.
        /// </summary>
        /// <param name="jobTaskDTO">jobTaskDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(JobTaskDTO jobTaskDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(jobTaskDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@maintTaskId", jobTaskDTO.JobTaskId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@taskName", jobTaskDTO.TaskName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@maintTaskGroupId", jobTaskDTO.JobTaskGroupId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@validateTag", (jobTaskDTO.ValidateTag == true ? "Y" : "N")));
            parameters.Add(dataAccessHandler.GetSQLParameter("@cardNumber", string.IsNullOrEmpty(jobTaskDTO.CardNumber) ? DBNull.Value : (object)jobTaskDTO.CardNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@cardId", jobTaskDTO.CardId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@remarksMandatory", (jobTaskDTO.RemarksMandatory == true? "Y":"N")));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", (jobTaskDTO.IsActive == true ? "Y" : "N")));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@updatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteid", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", jobTaskDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the maintenance task record to the database
        /// </summary>
        /// <param name="jobTaskDTO">jobTaskDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public JobTaskDTO InsertJobTask(JobTaskDTO jobTaskDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(jobTaskDTO, loginId, siteId);
            string insertMaintenanceTaskQuery = @"insert into Maint_Tasks 
                                                        (
                                                        TaskName,
                                                        MaintTaskGroupId,
                                                        ValidateTag,
                                                        CardNumber,
                                                        CardId,
                                                        RemarksMandatory,
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
                                                        @taskName,
                                                        @maintTaskGroupId,
                                                        @validateTag,
                                                        @cardNumber,
                                                        @cardId,
                                                        @remarksMandatory,
                                                        @isActive,
                                                        @createdBy,
                                                        Getdate(),
                                                        @updatedBy,
                                                        Getdate(),                                                        
                                                        Newid(),
                                                        @siteid ,
                                                        @masterEntityId
                                                        )SELECT * FROM Maint_Tasks WHERE MaintTaskId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertMaintenanceTaskQuery, GetSQLParameters(jobTaskDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshJobTaskDTO(jobTaskDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting jobTaskDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(jobTaskDTO);
            return jobTaskDTO;
        }

        /// <summary>
        /// Updates the maintenance task record
        /// </summary>
        /// <param name="jobTaskDTO">jobTaskDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public JobTaskDTO UpdateJobTask(JobTaskDTO jobTaskDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(jobTaskDTO, loginId, siteId);
            string updateMaintenanceTaskQuery = @"update Maint_Tasks 
                                         set TaskName = @taskName,
                                             MaintTaskGroupId=@maintTaskGroupId,
                                             ValidateTag=@validateTag,
                                             CardNumber=@cardNumber,
                                             CardId=@cardId,
                                             RemarksMandatory=@remarksMandatory,
                                             IsActive = @isActive,
                                             LastUpdatedBy = @updatedBy, 
                                             LastupdatedDate = Getdate(),
                                             --site_id=@siteid, 
                                             MasterEntityId=@masterEntityId
                                       where MaintTaskId = @maintTaskId
                                       SELECT * FROM Maint_Tasks WHERE MaintTaskId = @maintTaskId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateMaintenanceTaskQuery, GetSQLParameters(jobTaskDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshJobTaskDTO(jobTaskDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating jobTaskDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(jobTaskDTO);
            return jobTaskDTO;
        }
        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="jobTaskGroupDTO">jobTaskGroupDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshJobTaskDTO(JobTaskDTO jobTaskDTO, DataTable dt)
        {
            log.LogMethodEntry(jobTaskDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                jobTaskDTO.JobTaskId = Convert.ToInt32(dt.Rows[0]["MaintTaskId"]);
                jobTaskDTO.LastupdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                jobTaskDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                jobTaskDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                jobTaskDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                jobTaskDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                jobTaskDTO.Siteid = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to jobTaskDTO class type
        /// </summary>
        /// <param name="jobTaskDataRow">jobTaskDTO DataRow</param>
        /// <returns>Returns jobTaskDTO</returns>
        private JobTaskDTO GetJobTaskDTO(DataRow jobTaskDataRow)
        {
            log.LogMethodEntry(jobTaskDataRow);
            JobTaskDTO jobTaskDataObject = new JobTaskDTO(Convert.ToInt32(jobTaskDataRow["MaintTaskId"]),
                                            jobTaskDataRow["TaskName"].ToString(),
                                            jobTaskDataRow["MaintTaskGroupId"] == DBNull.Value ? -1 : Convert.ToInt32(jobTaskDataRow["MaintTaskGroupId"]), 
                                            jobTaskDataRow["ValidateTag"] == DBNull.Value ? false : (jobTaskDataRow["ValidateTag"].ToString() == "Y"? true: false),
                                            jobTaskDataRow["CardNumber"] == DBNull.Value ? null : jobTaskDataRow["CardNumber"].ToString(),                                            
                                            jobTaskDataRow["CardId"] == DBNull.Value ? -1 : Convert.ToInt32(jobTaskDataRow["CardId"]),
                                            jobTaskDataRow["RemarksMandatory"] == DBNull.Value ? false : (jobTaskDataRow["RemarksMandatory"].ToString() == "Y" ? true : false),
                                            jobTaskDataRow["IsActive"] == DBNull.Value ? false : (jobTaskDataRow["IsActive"].ToString() == "Y" ? true : false), 
                                            jobTaskDataRow["CreatedBy"].ToString(),
                                            jobTaskDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(jobTaskDataRow["CreationDate"]),
                                            jobTaskDataRow["LastUpdatedBy"].ToString(),
                                            jobTaskDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(jobTaskDataRow["LastupdatedDate"]),
                                            jobTaskDataRow["Guid"].ToString(),
                                            jobTaskDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(jobTaskDataRow["site_id"]),
                                            jobTaskDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(jobTaskDataRow["SynchStatus"]),
                                            jobTaskDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(jobTaskDataRow["MasterEntityId"])//Modification on 18-Jul-2016 for adding publish to site
                                            );
            log.LogMethodExit(jobTaskDataObject);
            return jobTaskDataObject;
        }

        /// <summary>
        /// Gets the JOb task data of passed maintenance task Id
        /// </summary>
        /// <param name="jobTaskId">integer type parameter</param>
        /// <returns>Returns JobTaskDTO</returns>
        public JobTaskDTO GetJobTask(int jobTaskId)
        {
            log.LogMethodEntry(jobTaskId);
            JobTaskDTO jobTaskDTO = null;
            string selectJobTaskQuery = SELECT_QUERY + @" WHERE MaintTaskId = @maintTaskId";
            SqlParameter parameter = new SqlParameter("@maintTaskId", jobTaskId);
            DataTable maintenanceJob = dataAccessHandler.executeSelectQuery(selectJobTaskQuery, new SqlParameter[] { parameter }, sqlTransaction);

            if (maintenanceJob.Rows.Count > 0)
            {
                jobTaskDTO = GetJobTaskDTO(maintenanceJob.Rows[0]);
            }
            log.LogMethodExit(jobTaskDTO);
            return jobTaskDTO;
        }

        /// <summary>
        /// Gets the JobTaskDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of JobTaskDTO matching the search criteria</returns>
        public List<JobTaskDTO> GetJobTaskList(List<KeyValuePair<JobTaskDTO.SearchByJobTaskParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectJobTaskQuery = SELECT_QUERY;
            if (searchParameters != null)
            {
                string joiner = " ";
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<JobTaskDTO.SearchByJobTaskParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count > 0) ? " and " : " ";

                        if (searchParameter.Key == JobTaskDTO.SearchByJobTaskParameters.JOB_TASK_ID 
                            || searchParameter.Key == JobTaskDTO.SearchByJobTaskParameters.JOB_TASK_GROUP_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == JobTaskDTO.SearchByJobTaskParameters.SITE_ID
                            || searchParameter.Key == JobTaskDTO.SearchByJobTaskParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == JobTaskDTO.SearchByJobTaskParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
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
                    selectJobTaskQuery = selectJobTaskQuery + query;
            }

            DataTable jobTaskData = dataAccessHandler.executeSelectQuery(selectJobTaskQuery, parameters.ToArray(), sqlTransaction);
            List<JobTaskDTO> jobTaskList = null;
            if (jobTaskData.Rows.Count > 0)
            {
                jobTaskList = new List<JobTaskDTO>();
                foreach (DataRow jobTaskDataRow in jobTaskData.Rows)
                {
                    JobTaskDTO jobTaskDataObject = GetJobTaskDTO(jobTaskDataRow);
                    jobTaskList.Add(jobTaskDataObject);
                } 
            }
            log.LogMethodExit(jobTaskList);
            return jobTaskList;
        }
    }
}
