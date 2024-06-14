/********************************************************************************************
 * Project Name - Scheduled Job
 * Description  - Bussiness logic of scheduled job
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By    Remarks          
 *********************************************************************************************
 *1.00        04-Feb-2016    Raghuveera     Created 
 *2.70.2        19-Sep-2019    Dakshakh       Modified : Added logs
 ********************************************************************************************/
using System;
using System.Collections.Generic;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// Bussiness logic for scheduled job
    /// </summary>
    public class ScheduledJob
    {
        private ScheduledJobDTO scheduledJobDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Default constructor
        /// </summary>
        public ScheduledJob()
        {
            log.LogMethodEntry();
            scheduledJobDTO = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="scheduledJobDTO">Parameter of the type ScheduledJobDTO</param>
        public ScheduledJob(ScheduledJobDTO scheduledJobDTO)
        {
            log.LogMethodEntry(scheduledJobDTO);
            this.scheduledJobDTO = scheduledJobDTO;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the scheduled job DTO
        /// </summary>
        public ScheduledJobDTO ScheduledJobDTO { get { return scheduledJobDTO; } }
    }
    /// <summary>
    /// Manages the list of Scheduled Job
    /// </summary>
    public class ScheduledJobList
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Returns the scheduled job list
        /// </summary>
        public List<ScheduledJobDTO> GetAllScheduledJobs(DateTime date)
        {
            log.LogMethodEntry(date);
            ScheduledJobDataHandler scheduledJobDataHandler = new ScheduledJobDataHandler();
            List<ScheduledJobDTO> scheduledJobDTOList = scheduledJobDataHandler.GetScheduledJobsList(date);
            log.LogMethodExit(scheduledJobDTOList);
            return scheduledJobDTOList;
            }
    }
}
