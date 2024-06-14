/********************************************************************************************
 * Project Name - Job Task
 * Description  - Bussiness logic of Job task
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        11-Jan-2016   Raghuveera     Created
 *2.70        08-Mar-2019   Guru S A       Renamed MaintenanceTask as JobTaskBL
 *2.70        23-Apr-2019   Mehraj         Added SaveJobTasks() method
              29-May-2019   Jagan Mohan    Code merge from Development to WebManagementStudio
 *2.70        07-Jul-2019   Dakshakh raj   Modified save function
 *2.80        07-Jun-2020   Girish Kundar  Modified :Added validation for duplicate task / task group mapping
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Maintenance
{
    /// <summary>
    /// Maintenance task class is creates and modifies the task details
    /// </summary>
    public class JobTaskBL
    {
        private JobTaskDTO jobTaskDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public JobTaskBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            jobTaskDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the id parameter
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="taskId">taskId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public JobTaskBL(ExecutionContext executionContext, int taskId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(executionContext, taskId, sqlTransaction);
            this.executionContext = executionContext;
            JobTaskDatahandler jobTaskDataHandler = new JobTaskDatahandler(null);
            this.jobTaskDTO = jobTaskDataHandler.GetJobTask(taskId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="jobTaskDTO">Parameter of the type jobTaskDTO</param>
        /// <param name="executionContext">executionContext</param>
        public JobTaskBL(ExecutionContext executionContext, JobTaskDTO jobTaskDTO)
        {
            log.LogMethodEntry(executionContext, jobTaskDTO);
            this.executionContext = executionContext;
            this.jobTaskDTO = jobTaskDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public JobTaskDTO JobTaskDTO
        {
            get
            {
                return jobTaskDTO;
            }
        }

        /// <summary>
        /// Saves the job tasks
        /// Checks if the tasks id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (jobTaskDTO.IsChanged == false
                 && jobTaskDTO.JobTaskId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            JobTaskDatahandler jobTaskDataHandler = new JobTaskDatahandler(sqlTransaction);
            if (jobTaskDTO.JobTaskId < 0)
            {
                Validate(sqlTransaction); // validate only if new record or update required
                jobTaskDTO = jobTaskDataHandler.InsertJobTask(jobTaskDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                jobTaskDTO.AcceptChanges();
            }
            else
            {
                if (jobTaskDTO.IsChanged)
                {
                    Validate(sqlTransaction);
                    jobTaskDTO = jobTaskDataHandler.UpdateJobTask(jobTaskDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    jobTaskDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        private void Validate(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            JobTaskDatahandler jobTaskDataHandler = new JobTaskDatahandler(sqlTransaction);
            List<KeyValuePair<JobTaskDTO.SearchByJobTaskParameters, string>> maintenanceTaskSearchParams = new List<KeyValuePair<JobTaskDTO.SearchByJobTaskParameters, string>>();
            maintenanceTaskSearchParams.Add(new KeyValuePair<JobTaskDTO.SearchByJobTaskParameters, string>(JobTaskDTO.SearchByJobTaskParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<JobTaskDTO> jobTaskDTOList = jobTaskDataHandler.GetJobTaskList(maintenanceTaskSearchParams);
            if (jobTaskDTOList != null && jobTaskDTOList.Any())
            {
                if (jobTaskDTOList.Exists(x => x.TaskName == jobTaskDTO.TaskName && x.JobTaskGroupId == jobTaskDTO.JobTaskGroupId && x.JobTaskId != jobTaskDTO.JobTaskId))
                {
                    log.Debug("Duplicate entries detail");
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2608, MessageContainerList.GetMessage(executionContext, " task / task group"), MessageContainerList.GetMessage(executionContext, "Validation Error")));
                }
            }
            log.LogMethodExit();
        }

    }


/// <summary>
/// Manages the list of job Task
/// </summary>
public class JobTaskList
    {
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<JobTaskDTO> jobTaskDTOList;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public JobTaskList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.jobTaskDTOList = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="jobTaskDTOList">jobTaskDTOList</param>
        public JobTaskList(ExecutionContext executionContext, List<JobTaskDTO> jobTaskDTOList)
        {
            log.LogMethodEntry(jobTaskDTOList, executionContext);
            this.jobTaskDTOList = jobTaskDTOList;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the jobs task list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<JobTaskDTO> GetAllJobTasks(List<KeyValuePair<JobTaskDTO.SearchByJobTaskParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            JobTaskDatahandler jobTaskDataHandler = new JobTaskDatahandler(sqlTransaction);
            List<JobTaskDTO> JobTaskDTOList = jobTaskDataHandler.GetJobTaskList(searchParameters);
            log.LogMethodExit(JobTaskDTOList);
            return JobTaskDTOList;
        }

        /// <summary>
        /// Save or update job tasks
        /// </summary>
        public void SaveJobTasks()
        {
            using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
            {
                try
                {
                    log.LogMethodEntry();
                    parafaitDBTrx.BeginTransaction();
                    if (jobTaskDTOList != null && jobTaskDTOList.Any())
                    {
                        foreach (JobTaskDTO jobTaskDTO in jobTaskDTOList)
                        {
                            JobTaskBL jobTaskBL = new JobTaskBL(executionContext, jobTaskDTO);
                            jobTaskBL.Save(parafaitDBTrx.SQLTrx);
                        }
                        parafaitDBTrx.EndTransaction();
                    }
                    log.LogMethodExit();
                }
                catch (SqlException ex)
                {
                    log.Error(ex);
                    parafaitDBTrx.RollBack();
                    if (ex.Number == 2601)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1872));
                    }
                    else if (ex.Number == 547)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                    }
                    else
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, ex.Message));
                    }
                }
                catch (ValidationException vlex)
                {
                    log.Error(vlex.Message, vlex);
                    parafaitDBTrx.RollBack();
                    log.LogMethodExit(vlex, vlex.Message);
                    throw;
                }
                catch (Exception ex)
                {
                    parafaitDBTrx.RollBack();
                    log.Error(ex.Message, ex);
                    log.LogMethodExit(ex, ex.Message);
                    throw;
                }
            }
        }

    }
}
