/********************************************************************************************
 * Project Name - Maintenance Schedule
 * Description  - Bussiness logic of the maintenance schedule
 *
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        18-Feb-2016   Raghuveera          Created 
 *2.70        08-Mar-2019   Guru S A            Renamed MaintenanceSchedule as JobScheduleBL
 *2.70.2        25-Jul-2019   Dakshakh raj        Modified : Save() method Insert/Update method returns DTO.
 *2.100        11-Aug-2020      Mushahid Faizan     Modified : Constructor, Save() method, Added Validate, Build() to get child records and 
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
    /// Maintenance schedule is creates and edits the maintenance schedule details
    /// </summary>
    public class JobScheduleBL : ScheduleCalendarBL
    {
        private JobScheduleDTO jobScheduleDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // private ExecutionContext executionContext;
        /// <summary>
        /// Default constructor
        /// </summary>
        public JobScheduleBL(ExecutionContext executionContext) 
            :base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            jobScheduleDTO = null; 
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with id parameter
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="jobScheduleId"></param>
        public JobScheduleBL(ExecutionContext executionContext, int jobScheduleId, SqlTransaction sqlTransaction = null)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext, jobScheduleId);
            JobScheduleDataHandler jobScheduleDataHandler = new JobScheduleDataHandler(sqlTransaction);
            jobScheduleDTO = jobScheduleDataHandler.GetJobScheduleDTO(jobScheduleId);
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="maintenanceScheduleDTO">Maintenance Schedule DTO</param>
        public JobScheduleBL(ExecutionContext executionContext, JobScheduleDTO maintenanceScheduleDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, maintenanceScheduleDTO);
            this.jobScheduleDTO = maintenanceScheduleDTO; 
            log.LogMethodExit();
        }
        /// <summary>
        /// Get JobScheduleDTO
        /// </summary>
        public JobScheduleDTO JobScheduleDTO
        {
            get { return jobScheduleDTO; }
        }
        /// <summary>
        /// Overridden method from base class
        /// </summary>
        public override void Save(SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry();
            SaveShedule(sqlTrx);
            log.LogMethodExit();
        }
        /// <summary>
        /// Saves the Maintenance Schedule
        /// Checks if the Maintenance schedule id is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        public void SaveShedule(SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(sqlTrx);
            JobScheduleDataHandler jobScheduleDataHandler = new JobScheduleDataHandler(sqlTrx);
            if (jobScheduleDTO.IsChanged == false && jobScheduleDTO.JobScheduleId > -1)
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

            if (jobScheduleDTO.JobScheduleId < 0)
            {
                jobScheduleDTO = jobScheduleDataHandler.InsertJobSchedule(jobScheduleDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                jobScheduleDTO.AcceptChanges();
            }
            else
            {
                if (jobScheduleDTO.IsChanged)
                {
                    jobScheduleDTO = jobScheduleDataHandler.UpdateJobSchedule(jobScheduleDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    jobScheduleDTO.AcceptChanges();
                }
            }
            SaveJobScheduleTask(sqlTrx);
            jobScheduleDTO.AcceptChanges();
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the child records 
        /// </summary>
        /// <param name="sqlTransaction"></param>
        private void SaveJobScheduleTask(SqlTransaction sqlTransaction)
        {
            if (jobScheduleDTO.JobScheduleTasksDTOList != null &&
                jobScheduleDTO.JobScheduleTasksDTOList.Any())
            {
                List<JobScheduleTasksDTO> updatedJobScheduleTasksDTOList = new List<JobScheduleTasksDTO>();
                foreach (var jobScheduleTasksDTO in jobScheduleDTO.JobScheduleTasksDTOList)
                {
                    if (jobScheduleTasksDTO.JobScheduleId != jobScheduleTasksDTO.JobScheduleId)
                    {
                        jobScheduleTasksDTO.JobScheduleId = jobScheduleTasksDTO.JobScheduleId;
                    }
                    if (jobScheduleTasksDTO.IsChanged)
                    {
                        updatedJobScheduleTasksDTOList.Add(jobScheduleTasksDTO);
                    }
                }
                if (updatedJobScheduleTasksDTOList.Any())
                {
                    JobScheduleTasksListBL jobScheduleTasksListBL = new JobScheduleTasksListBL(executionContext, updatedJobScheduleTasksDTOList);
                    jobScheduleTasksListBL.Save(sqlTransaction);
                }
            }
        }

        /// <summary>
        /// Validate the scheduleDTO
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
    /// Manages the list of maintenance Schedule
    /// </summary>
    public class JobScheduleListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<JobScheduleDTO> jobScheduleDTOList = new List<JobScheduleDTO>();

        /// <summary>
        /// create the scheduleExclusionDTOs list object
        /// </summary>
        public JobScheduleListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext; 
            log.LogMethodExit();
        }

        /// <summary>
        /// Paramatereized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public JobScheduleListBL(ExecutionContext executionContext, List<JobScheduleDTO> jobScheduleDTOList) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, jobScheduleDTOList);
            this.jobScheduleDTOList = jobScheduleDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the maintenance Schedule list
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<JobScheduleDTO> GetAllJobScheduleDTOList(List<KeyValuePair<JobScheduleDTO.SearchByJobScheduleDTOParameters, string>> searchParameters,
             bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            JobScheduleDataHandler jobScheduleDataHandler = new JobScheduleDataHandler(null);
            List<JobScheduleDTO> jobScheduleDTOList = jobScheduleDataHandler.GetJobScheduleDTOList(searchParameters);
            if (jobScheduleDTOList != null && jobScheduleDTOList.Any() && loadChildRecords)
            {
                Build(jobScheduleDTOList, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(jobScheduleDTOList);
            return jobScheduleDTOList;
        }

        /// <summary>
        /// Gets the jobScheduleDTO List for scheduleIdList
        /// </summary>
        /// <param name="scheduleIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of jobScheduleDTO</returns>
        public List<JobScheduleDTO> GetAllJobScheduleDTOList(List<int> scheduleIdList, bool loadChildRecords = true, bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(scheduleIdList, activeRecords);
            JobScheduleDataHandler jobScheduleDataHandler = new JobScheduleDataHandler(sqlTransaction);
            this.jobScheduleDTOList = jobScheduleDataHandler.GetJobScheduleDTOList(scheduleIdList, activeRecords);
            if (jobScheduleDTOList != null && jobScheduleDTOList.Any() && loadChildRecords)
            {
                Build(jobScheduleDTOList, activeRecords, sqlTransaction);
            }
            log.LogMethodExit(jobScheduleDTOList);
            return jobScheduleDTOList;
        }

        private void Build(List<JobScheduleDTO> jobScheduleDTOList, bool activeChildRecords, SqlTransaction sqlTransaction)
        {
            Dictionary<int, JobScheduleDTO> jobScheduleDTODictionary = new Dictionary<int, JobScheduleDTO>();
            List<int> jobScheduleIdList = new List<int>();
            for (int i = 0; i < jobScheduleDTOList.Count; i++)
            {
                if (jobScheduleDTODictionary.ContainsKey(jobScheduleDTOList[i].JobScheduleId))
                {
                    continue;
                }
                jobScheduleDTODictionary.Add(jobScheduleDTOList[i].JobScheduleId, jobScheduleDTOList[i]);
                jobScheduleIdList.Add(jobScheduleDTOList[i].JobScheduleId);
            }
            JobScheduleTasksListBL jobScheduleTasksListBL = new JobScheduleTasksListBL(executionContext);
            List<JobScheduleTasksDTO> jobScheduleTasksDTOList = jobScheduleTasksListBL.GetAllJobScheduleTaskDTOList(jobScheduleIdList, activeChildRecords, sqlTransaction);
            if (jobScheduleTasksDTOList != null && jobScheduleTasksDTOList.Any())
            {
                for (int i = 0; i < jobScheduleTasksDTOList.Count; i++)
                {
                    if (jobScheduleDTODictionary.ContainsKey(jobScheduleTasksDTOList[i].JobScheduleId) == false)
                    {
                        continue;
                    }
                    JobScheduleDTO jobScheduleDTO = jobScheduleDTODictionary[jobScheduleTasksDTOList[i].JobScheduleId];
                    if (jobScheduleDTO.JobScheduleTasksDTOList == null)
                    {
                        jobScheduleDTO.JobScheduleTasksDTOList = new List<JobScheduleTasksDTO>();
                    }
                    jobScheduleDTO.JobScheduleTasksDTOList.Add(jobScheduleTasksDTOList[i]);
                }
            }
        }

        /// <summary>
        /// Saves the jobScheduleDTOList List
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (jobScheduleDTOList == null ||
                jobScheduleDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < jobScheduleDTOList.Count; i++)
            {
                var jobScheduleDTO = jobScheduleDTOList[i];
                if (jobScheduleDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    JobScheduleBL jobScheduleBL = new JobScheduleBL(executionContext, jobScheduleDTO);
                    jobScheduleBL.Save(sqlTransaction);
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
                    log.LogVariableState("jobScheduleDTO", jobScheduleDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}
