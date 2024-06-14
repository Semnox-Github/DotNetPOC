/********************************************************************************************
 * Project Name - Schedule Asset Task
 * Description  - Schedule Asset Task business logics 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        18-Jan-2016   Raghuveera          Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// schedule asset task saves the maping record of schedule vs asset and task.
    /// </summary>
    public class ScheduleAssetTask
    {
         Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ScheduleAssetTaskDTO scheduleAssetTaskDTO;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ScheduleAssetTask()
        {
            log.Debug("starts-ScheduleAssetTask() default constructor");
            scheduleAssetTaskDTO = null;
            log.Debug("Ends-ScheduleAssetTask() default constructor");
        }

        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="scheduleAssetTaskDTO">Schedule Exclusion DTO</param>
        public ScheduleAssetTask(ScheduleAssetTaskDTO scheduleAssetTaskDTO)
        {
            log.Debug("starts-ScheduleAssetTask(scheduleAssetTaskDTO) parameterised constructor");
            this.scheduleAssetTaskDTO = scheduleAssetTaskDTO;
            log.Debug("Ends-ScheduleAssetTask(scheduleAssetTaskDTO) parameterised constructor");
        }
        /// <summary>
        /// Saves the schedule asset task record
        /// Checks if the tasks id is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        public void Save()
        {
            log.Debug("starts-Save() method");
             ExecutionContext machineUserContext =  ExecutionContext.GetExecutionContext();
            ScheduleAssetTaskDataHandler scheduleAssetTaskDataHandler = new ScheduleAssetTaskDataHandler();
            if (scheduleAssetTaskDTO.MaintSchAssetTaskId < 0)
            {
                int MaintSchAssetTaskId = scheduleAssetTaskDataHandler.InsertScheduleAssetTask(scheduleAssetTaskDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                scheduleAssetTaskDTO.MaintSchAssetTaskId = MaintSchAssetTaskId;
            }
            else
            {
                if (scheduleAssetTaskDTO.IsChanged == true)
                {
                    scheduleAssetTaskDataHandler.UpdateScheduleAssetTask(scheduleAssetTaskDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                    scheduleAssetTaskDTO.AcceptChanges();
                }
            }
            log.Debug("Ends-Save() method");
        }

    }
    /// <summary>
    /// Manages the list of Schedule Asset Task
    /// </summary>
    public class ScheduleAssetTaskList
    {
         Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Returns the Schedule asset task list
        /// </summary>
        public List<ScheduleAssetTaskDTO> GetAllScheduleAssetTasks(List<KeyValuePair<ScheduleAssetTaskDTO.SearchByScheduleAssetTaskParameters, string>> searchParameters)
        {
            log.Debug("Start-GetAllScheduleAssetTasks(searchParameters) method");
            ScheduleAssetTaskDataHandler scheduleAssetTaskDataHandler = new ScheduleAssetTaskDataHandler();
            log.Debug("End-GetAllScheduleAssetTasks(searchParameters) method");
            return scheduleAssetTaskDataHandler.GetScheduleAssetTaskList(searchParameters);
        }
    }
}
