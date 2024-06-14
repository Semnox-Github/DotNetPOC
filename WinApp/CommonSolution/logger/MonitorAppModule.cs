/********************************************************************************************
 * Project Name - Monitor App Module
 * Description  - Bussiness logic of monitors app module
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.60.2      13-June-2019   Jagan Mohana Rao    Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System.Collections.Generic;

namespace Semnox.Parafait.logger
{
    /// <summary>
    /// Monitor Applications module
    /// </summary>
    public class MonitorAppModule
    {
        private MonitorAppModuleDTO monitorAppModuleDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        private MonitorAppModule(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="monitorAppModuleDTO"></param>
        public MonitorAppModule(ExecutionContext executionContext, MonitorAppModuleDTO monitorAppModuleDTO)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext, monitorAppModuleDTO);
            this.monitorAppModuleDTO = monitorAppModuleDTO;
            log.LogMethodExit();
        }
        /// <summary>
        /// Saves the monitor application module
        /// Monitor application moduleId will be inserted if applicationId is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save()
        {
            log.LogMethodEntry();
            MonitorAppModuleDataHandler monitorAppModuleDataHandler = new MonitorAppModuleDataHandler();
            if (monitorAppModuleDTO.ModuleId <= 0)
            {
                monitorAppModuleDTO = monitorAppModuleDataHandler.Insert(monitorAppModuleDTO, executionContext.GetUserId(), executionContext.GetSiteId());
            }
            else
            {
                if (monitorAppModuleDTO.IsChanged == true)
                {
                    monitorAppModuleDTO = monitorAppModuleDataHandler.Update(monitorAppModuleDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    monitorAppModuleDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of monitor application module 
    /// </summary>
    public class MonitorAppModuleList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<MonitorAppModuleDTO> monitorAppModuleDTOList;
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public MonitorAppModuleList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.monitorAppModuleDTOList = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the monitor application module list
        /// </summary>
        public List<MonitorAppModuleDTO> GetAllMonitorAppModuleDTO(List<KeyValuePair<MonitorAppModuleDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            MonitorAppModuleDataHandler MonitorAppModuleDataHandler = new MonitorAppModuleDataHandler();
            monitorAppModuleDTOList = MonitorAppModuleDataHandler.GetAllMonitorAppModuleDTO(searchParameters);
            log.LogMethodExit(monitorAppModuleDTOList);
            return monitorAppModuleDTOList;
        }
    }
}
