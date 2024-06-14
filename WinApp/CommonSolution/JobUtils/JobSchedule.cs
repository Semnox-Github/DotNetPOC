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
    /// Bussiness logic for job schedule
    /// </summary>
    public class JobSchedule
    {
        private JobScheduleDTO jobScheduleDTO;
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor
        /// </summary>
        public JobSchedule()
        {
            log.Debug("Starts-JobSchedule() default constructor.");
            jobScheduleDTO = null;
            log.Debug("Ends-JobSchedule() default constructor.");
        }

        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="jobScheduleDTO">JobScheduleDTO</param>
        public JobSchedule(JobScheduleDTO jobScheduleDTO)
        {
            log.Debug("Starts-JobSchedule(jobScheduleDTO) parameterized constructor.");
            this.jobScheduleDTO = jobScheduleDTO;
            log.Debug("Ends-JobSchedule(jobScheduleDTO) parameterized constructor.");
        }

        /// <summary>
        /// Saves the job schedule 
        /// Checks if the job id is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        public void Save()
        {
            log.Debug("Starts-Save() method.");
            ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
            JobScheduleDataHandler assetDataHandler = new JobScheduleDataHandler();
            if (jobScheduleDTO.JobId < 0)
            {
                int jobId = assetDataHandler.InsertJobSchedule(jobScheduleDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                jobScheduleDTO.JobId = jobId;
            }
            else
            {
                if (jobScheduleDTO.IsChanged == true)
                {
                    assetDataHandler.UpdateJobSchedule(jobScheduleDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                    jobScheduleDTO.AcceptChanges();
                }
            }
            log.Debug("Ends-Save() method.");
        }
        /// <summary>
        /// Returns the job schedule DTO
        /// </summary>
        public JobScheduleDTO JobScheduleDTO { get { return jobScheduleDTO; } }
    }
    /// <summary>
    /// Manages the list of Job Schedule DTOs
    /// </summary>
    public class JobScheduleList
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Returns the job schedule list
        /// </summary>
        public List<JobScheduleDTO> GetAllJobSchedule()
        {
            log.Debug("Starts-GetAllJobSchedule() method.");
            JobScheduleDataHandler jobScheduleDataHandler = new JobScheduleDataHandler();
            log.Debug("Ends-GetAllJobSchedule() method by returning the result of jobScheduleDataHandler.GetJobScheduleList() call.");
            return jobScheduleDataHandler.GetJobScheduleList();
        }
    }
}
