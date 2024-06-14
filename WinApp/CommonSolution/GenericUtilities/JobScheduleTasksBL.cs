/********************************************************************************************
 * Project Name - Job schedule Task
 * Description  - Job schedule Task business logics 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        18-Jan-2016   Raghuveera          Created 
 *2.70        11-Mar-2019   Guru S A            Rename ScheduleAssetTask as JobScheduleTasksBL
 *2.70.2       25-Jul-2019   Dakshakh raj        Modified : Save() method Insert/Update method returns DTO.
 *2.100        11-Aug-2020      Mushahid Faizan     Modified : Constructor, Save() method, Added Validate, and 
 *                                                 List class changes as per 3 tier standards.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// job schedule task saves the maping record of schedule vs asset and task.
    /// </summary>
    public class JobScheduleTasksBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private JobScheduleTasksDTO jobScheduleTasksDTO;
        private ExecutionContext executionContext;

        /// <summary>
        ///  Default constructor
        /// </summary>
        /// <param name="executionContext"></param>
        private JobScheduleTasksBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            jobScheduleTasksDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the Id parameter
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="jobScheduleTasksId"></param>
        public JobScheduleTasksBL(ExecutionContext executionContext, int jobScheduleTasksId)
        {
            log.LogMethodEntry(executionContext, jobScheduleTasksDTO);
            this.executionContext = executionContext;
            JobScheduleTasksDataHandler jobScheduleTasksDataHandler = new JobScheduleTasksDataHandler(null);
            this.jobScheduleTasksDTO = jobScheduleTasksDataHandler.GetJobScheduleTaskDTO(jobScheduleTasksId);
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="jobScheduleTasksDTO"></param>
        public JobScheduleTasksBL(ExecutionContext executionContext, JobScheduleTasksDTO jobScheduleTasksDTO)
        {
            log.LogMethodEntry(executionContext, jobScheduleTasksDTO);
            this.executionContext = executionContext;
            this.jobScheduleTasksDTO = jobScheduleTasksDTO;
            log.LogMethodExit();
        }
        /// <summary>
        /// Saves the job schedule task record
        /// Checks if the tasks id is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        public void Save(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(); 
            JobScheduleTasksDataHandler jobScheduleTasksDataHandler = new JobScheduleTasksDataHandler(sqlTrx);

            if (jobScheduleTasksDTO.IsChanged == false && jobScheduleTasksDTO.JobScheduleTaskId > -1)
            {
                log.LogMethodExit(null, "No Changes to save");
                return;
            }
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }

            if (jobScheduleTasksDTO.JobScheduleTaskId < 0)
            {
                jobScheduleTasksDTO = jobScheduleTasksDataHandler.InsertJobScheduleTask(jobScheduleTasksDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                jobScheduleTasksDTO.AcceptChanges();
            }
            else
            {
                if (jobScheduleTasksDTO.IsChanged)
                {
                    jobScheduleTasksDTO = jobScheduleTasksDataHandler.UpdateJobScheduleTask(jobScheduleTasksDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    jobScheduleTasksDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validate the jobScheduleTasksDTO
        /// </summary>
        /// <returns></returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            // Validation Logic here.
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

    }
    /// <summary>
    /// Manages the list of Job schedule Task
    /// </summary>
    public class JobScheduleTasksListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<JobScheduleTasksDTO> jobScheduleTasksDTOList = new List<JobScheduleTasksDTO>();

        /// <summary>
        ///  Default constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public JobScheduleTasksListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext; 
            log.LogMethodExit();
        }
        /// <summary>
        ///  Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public JobScheduleTasksListBL(ExecutionContext executionContext, List<JobScheduleTasksDTO> jobScheduleTasksDTOList) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, jobScheduleTasksDTOList);
            this.jobScheduleTasksDTOList = jobScheduleTasksDTOList;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the Job schedule task list
        /// </summary>
        public List<JobScheduleTasksDTO> GetAllJobScheduleTaskDTOList(List<KeyValuePair<JobScheduleTasksDTO.SearchByJobScheduleTaskParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            JobScheduleTasksDataHandler jobScheduleTasksDataHandler = new JobScheduleTasksDataHandler(null);
            List<JobScheduleTasksDTO> jobScheduleTasksDTOList = jobScheduleTasksDataHandler.GetJobScheduleTaskDTOList(searchParameters);
            log.LogMethodExit(jobScheduleTasksDTOList);
            return jobScheduleTasksDTOList;
        }

        /// <summary>
        /// Gets the jobScheduleDTO List for scheduleIdList
        /// </summary>
        /// <param name="jobScheduleIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of jobScheduleDTO</returns>
        public List<JobScheduleTasksDTO> GetAllJobScheduleTaskDTOList(List<int> jobScheduleIdList, bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(jobScheduleIdList, activeRecords);
            JobScheduleTasksDataHandler jobScheduleTasksDataHandler = new JobScheduleTasksDataHandler(sqlTransaction);
            List<JobScheduleTasksDTO> jobScheduleTasksDTOList = jobScheduleTasksDataHandler.GetJobScheduleTaskDTOList(jobScheduleIdList, activeRecords);
            log.LogMethodExit(jobScheduleTasksDTOList);
            return jobScheduleTasksDTOList;
        }

        /// <summary>
        /// Saves the jobScheduleTasksDTOList List
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (jobScheduleTasksDTOList == null ||
                jobScheduleTasksDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < jobScheduleTasksDTOList.Count; i++)
            {
                var jobScheduleTasksDTO = jobScheduleTasksDTOList[i];
                if (jobScheduleTasksDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    JobScheduleTasksBL jobScheduleTasksBL = new JobScheduleTasksBL(executionContext, jobScheduleTasksDTO);
                    jobScheduleTasksBL.Save(sqlTransaction);
                }
                catch (SqlException sqlEx)
                {
                    log.Error(sqlEx);
                    log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                    if (sqlEx.Number == 547)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (ValidationException valEx)
                {
                    log.Error(valEx);
                    log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                    throw;
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving jobScheduleDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("jobScheduleDTO", jobScheduleTasksDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}
