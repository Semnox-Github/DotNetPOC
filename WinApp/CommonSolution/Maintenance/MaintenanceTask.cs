/********************************************************************************************
 * Project Name - Maintenance Task
 * Description  - Bussiness logic of maintenance task
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        11-Jan-2016   Raghuveera          Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Maintenance
{
    /// <summary>
    /// Maintenance task class is creates and modifies the task details
    /// </summary>
    public class MaintenanceTask
    {
        private MaintenanceTaskDTO maintenanceTaskDTO;
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Default constructor
        /// </summary>
        public MaintenanceTask()
        {
            log.Debug("Starts-MaintenanceTask() default constructor");
            maintenanceTaskDTO = null;
            log.Debug("Ends-MaintenanceTask() default constructor");
        }
        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="maintenanceTaskDTO">Parameter of the type MaintenanceTaskDTO</param>
        public MaintenanceTask(MaintenanceTaskDTO maintenanceTaskDTO)
        {
            log.Debug("Starts-MaintenanceTask(maintenanceTaskDTO) parameterized constructor.");
            this.maintenanceTaskDTO = maintenanceTaskDTO;
            log.Debug("Ends-MaintenanceTask(maintenanceTaskDTO) parameterized constructor.");
        }
        /// <summary>
        /// Saves the maintenance tasks
        /// Checks if the tasks id is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        public void Save()
        {
            log.Debug("Starts-Save() method.");
            ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
            MaintenanceTaskDataHandler maintenanceTaskDataHandler = new MaintenanceTaskDataHandler();
            if (maintenanceTaskDTO.MaintTaskId < 0)
            {
                int maintenanceTaskId = maintenanceTaskDataHandler.InsertMaintenanceTask(maintenanceTaskDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                maintenanceTaskDTO.MaintTaskId = maintenanceTaskId;
            }
            else
            {
                if (maintenanceTaskDTO.IsChanged == true)
                {
                    maintenanceTaskDataHandler.UpdateMaintenanceTask(maintenanceTaskDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                    maintenanceTaskDTO.AcceptChanges();
                }
            }
            log.Debug("Ends-Save() method.");
        }

    }
    /// <summary>
    /// Manages the list of Maintenance Task
    /// </summary>
    public class MaintenanceTaskList
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Returns the maintenance task
        /// </summary>
        public MaintenanceTaskDTO GetMaintenanceTask(int taskId)
        {
            log.Debug("Starts-GetMaintenanceTask(int taskId) method.");
            MaintenanceTaskDataHandler maintenanceTaskDataHandler = new MaintenanceTaskDataHandler();
            log.Debug("Ends-GetMaintenanceTask(int taskId) method by returning the result of maintenanceTaskDataHandler.GetMaintenanceTask(taskId) call.");
            return maintenanceTaskDataHandler.GetMaintenanceTask(taskId);
        }
        /// <summary>
        /// Returns the maintenance task list
        /// </summary>
        public List<MaintenanceTaskDTO> GetAllMaintenanceTasks(List<KeyValuePair<MaintenanceTaskDTO.SearchByMaintenanceTaskParameters, string>> searchParameters)
        {
            log.Debug("Starts-GetAllMaintenanceTasks(searchParameters) method.");
            MaintenanceTaskDataHandler maintenanceTaskDataHandler = new MaintenanceTaskDataHandler();
            log.Debug("Ends-GetAllMaintenanceTasks(searchParameters) method by returning the result of maintenanceTaskDataHandler.GetMaintenanceTaskList(searchParameters) call.");
            return maintenanceTaskDataHandler.GetMaintenanceTaskList(searchParameters);
        }
    }
}
