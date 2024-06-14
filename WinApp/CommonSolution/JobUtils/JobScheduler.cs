/********************************************************************************************
 * Project Name - Job Schedule
 * Description  - Bussiness logic of job schedule
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        05-Feb-2016   Raghuveera          Created 
 *2.70       11-Mar-2019   Guru S A            Booking phase2 changes
 *2.140      14-Sep-2021      Fiona           Modified: Issue fix 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// Bussiness logic for job scheduler
    /// </summary>
    public class JobSchedulerBL
    {
        private JobSchedulerDTO jobSchedulerDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor
        /// </summary>
        public JobSchedulerBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            jobSchedulerDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the Id parameter
        /// </summary>
        /// <param name="jobScheduleDTO">JobScheduleDTO</param>
        public JobSchedulerBL(ExecutionContext executionContext, int jobSchedulerId)
        {
            log.LogMethodEntry(executionContext, jobSchedulerId);
            this.executionContext = executionContext;
            JobSchedulerDataHandler jobSchedulerDataHandler = new JobSchedulerDataHandler();
            this.jobSchedulerDTO = jobSchedulerDataHandler.GetJobSchedulerDTO(jobSchedulerId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="jobScheduleDTO">JobScheduleDTO</param>
        public JobSchedulerBL(ExecutionContext executionContext, JobSchedulerDTO jobScheduleDTO)
        {
            log.LogMethodEntry(executionContext, jobScheduleDTO);
            this.executionContext = executionContext;
            this.jobSchedulerDTO = jobScheduleDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the job schedule 
        /// Checks if the job id is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        public void Save()
        {
            log.LogMethodEntry(); 
            JobSchedulerDataHandler jobSchedulerDataHandler = new JobSchedulerDataHandler();
            if (jobSchedulerDTO.JobId < 0)
            {
                int jobId = jobSchedulerDataHandler.InsertJobScheduler(jobSchedulerDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                jobSchedulerDTO.JobId = jobId;
                jobSchedulerDTO.AcceptChanges();
            }
            else
            {
                if (jobSchedulerDTO.IsChanged == true)
                {
                    jobSchedulerDataHandler.UpdateJobScheduler(jobSchedulerDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    jobSchedulerDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the job scheduler DTO
        /// </summary>
        public JobSchedulerDTO JobSchedulerDTO { get { return jobSchedulerDTO; } }
    }
    /// <summary>
    /// Manages the list of Job Schedule DTOs
    /// </summary>
    public class JobSchedulerListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        /// <summary>
        ///  Default constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public JobSchedulerListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext; 
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the job scheduler list
        /// </summary>
        public List<JobSchedulerDTO> GetAllJobSchedulerDTOList()
        {
            log.LogMethodEntry();
            JobSchedulerDataHandler jobSchedulerDataHandler = new JobSchedulerDataHandler();
            List<JobSchedulerDTO> jobSchedulerDTOList = jobSchedulerDataHandler.GetJobSchedulerDTOList();
            log.LogMethodExit(jobSchedulerDTOList);
            return jobSchedulerDTOList;

        }
    }
}
