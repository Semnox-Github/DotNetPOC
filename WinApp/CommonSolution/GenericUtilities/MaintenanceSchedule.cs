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
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Maintenance schedule is creates and edits the maintenance schedule details
    /// </summary>
    public class MaintenanceSchedule : Schedule
    {
        private MaintenanceScheduleDTO maintenanceScheduleDTO;
         Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       // private ExecutionContext executionContext;
        /// <summary>
        /// Default constructor
        /// </summary>
        public MaintenanceSchedule(ExecutionContext executionContext) 
            :base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            maintenanceScheduleDTO = null; 
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="maintenanceScheduleDTO">Maintenance Schedule DTO</param>
        public MaintenanceSchedule(ExecutionContext executionContext, MaintenanceScheduleDTO maintenanceScheduleDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, maintenanceScheduleDTO);
            this.maintenanceScheduleDTO = maintenanceScheduleDTO; 
            log.LogMethodExit();
        }
        /// <summary>
        /// Overridden method from base class
        /// </summary>
        public override void Save()
        {
            log.LogMethodEntry();
            SaveShedule();
            log.LogMethodExit();
        }
        /// <summary>
        /// Saves the Maintenance Schedule
        /// Checks if the Maintenance schedule id is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        public void SaveShedule()
        {
            log.Debug("starts-Save() method"); 
            MaintenanceScheduleDataHandler maintenanceScheduleDataHandler = new MaintenanceScheduleDataHandler();
            if (maintenanceScheduleDTO.MaintScheduleId < 0)
            {
                int maintScheduleId = maintenanceScheduleDataHandler.InsertMaintenanceSchedule(maintenanceScheduleDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                maintenanceScheduleDTO.MaintScheduleId = maintScheduleId;
            }
            else
            {
                if (maintenanceScheduleDTO.IsChanged == true)
                {
                    maintenanceScheduleDataHandler.UpdateMaintenanceSchedule(maintenanceScheduleDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    maintenanceScheduleDTO.AcceptChanges();
                }
            }
            log.Debug("Ends-Save() method");
        }
    }
    /// <summary>
    /// Manages the list of maintenance Schedule
    /// </summary>
    public class MaintenanceScheduleList
    {
         Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Returns the maintenance schedule
        /// </summary>
        public MaintenanceScheduleDTO GetMaintSchedule(int maintScheduleId)
        {
            log.Debug("Starts-GetMaintSchedule(maintScheduleId) method.");
            MaintenanceScheduleDataHandler maintenanceScheduleDataHandler = new MaintenanceScheduleDataHandler();
            log.Debug("Ends-GetMaintSchedule(maintScheduleId) method by returning the result of maintenanceScheduleDataHandler.GetMaintenanceSchedule(maintScheduleId) call.");
            return maintenanceScheduleDataHandler.GetMaintenanceSchedule(maintScheduleId);
        }
        /// <summary>
        /// Returns the maintenance Schedule list
        /// </summary>
        public List<MaintenanceScheduleDTO> GetAllMaintenanceSchedule(List<KeyValuePair<MaintenanceScheduleDTO.SearchByMaintenanceScheduleParameters, string>> searchParameters)
        {
            log.Debug("Starts-GetAllMaintenanceSchedule(searchParameters) method");
            MaintenanceScheduleDataHandler maintenanceScheduleDataHandler = new MaintenanceScheduleDataHandler();
            log.Debug("Ends-GetAllMaintenanceSchedule(searchParameters) method by returning the result ofmaintenanceScheduleDataHandler.GetMaintenanceScheduleList(searchParameters) call");
            return maintenanceScheduleDataHandler.GetMaintenanceScheduleList(searchParameters);
        }
    }
}
