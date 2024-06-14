/********************************************************************************************
 * Project Name - Maintenance Task Group
 * Description  - Bussiness logic of maintenance task group
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
    ///Bussiness logic of maintenance task group. It creates and modifies the maintenance task group details
    /// </summary>
    public class MaintenanceTaskGroup
    {
        private MaintenanceTaskGroupDTO maintenanceTaskGroupDTO;

        /// <summary>
        /// Default constructor
        /// </summary>
        public MaintenanceTaskGroup()
        {
            maintenanceTaskGroupDTO = null;
        }

        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="maintenanceTaskGroupDTO">Parameter of the type MaintenanceTaskGroupDTO</param>
        public MaintenanceTaskGroup(MaintenanceTaskGroupDTO maintenanceTaskGroupDTO)
        {
            this.maintenanceTaskGroupDTO = maintenanceTaskGroupDTO;
        }
        /// <summary>
        /// Saves the maintenance tasks group
        /// Checks if the tasks id is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        public void Save()
        {
            ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
            MaintenanceTaskGroupDataHandler maintenanceTaskGroupDataHandler = new MaintenanceTaskGroupDataHandler();
            if (maintenanceTaskGroupDTO.MaintTaskGroupId < 0)
            {
                int maintenanceTaskGroupId = maintenanceTaskGroupDataHandler.InsertMaintenanceTaskGroup(maintenanceTaskGroupDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                maintenanceTaskGroupDTO.MaintTaskGroupId = maintenanceTaskGroupId;
            }
            else
            {
                if (maintenanceTaskGroupDTO.IsChanged == true)
                {
                    maintenanceTaskGroupDataHandler.UpdateMaintenanceTaskGroup(maintenanceTaskGroupDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                    maintenanceTaskGroupDTO.AcceptChanges();
                }
            }
        }
    }
    /// <summary>
    /// Manages the list of maintenance task group
    /// </summary>
    public class MaintenanceTaskGroupList
    {
        /// <summary>
        /// Returns the maintenance task group list
        /// </summary>
        public List<MaintenanceTaskGroupDTO> GetAllMaintenanceTaskGroups(List<KeyValuePair<MaintenanceTaskGroupDTO.SearchByMaintenanceTaskGroupParameters, string>> searchParameters)
        {
            MaintenanceTaskGroupDataHandler maintenanceTaskGroupDataHandler = new MaintenanceTaskGroupDataHandler();
            return maintenanceTaskGroupDataHandler.GetMaintenanceTaskGroupList(searchParameters);
        }
    }
}
