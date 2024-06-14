/********************************************************************************************
 * Project Name - Monitor log status
 * Description  - Bussiness logic of monitors
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
    /// Monitor Log Status
    /// </summary>
    public class MonitorLogStatus
    {
        private MonitorLogStatusDTO monitorLogStatusDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        private MonitorLogStatus(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="monitorLogStatusDTO"></param>
        public MonitorLogStatus(ExecutionContext executionContext, MonitorLogStatusDTO monitorLogStatusDTO)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext, monitorLogStatusDTO);
            this.monitorLogStatusDTO = monitorLogStatusDTO;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Saves the monitor application module
        /// Monitor log status will be inserted if statusId is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save()
        {
            log.LogMethodEntry();
            MonitorLogStatusDataHandler monitorLogStatusDataHandler = new MonitorLogStatusDataHandler();
            if (monitorLogStatusDTO.StatusId <= 0)
            {
                monitorLogStatusDTO = monitorLogStatusDataHandler.Insert(monitorLogStatusDTO, executionContext.GetUserId(), executionContext.GetSiteId());
            }
            else
            {
                if (monitorLogStatusDTO.IsChanged == true)
                {
                    monitorLogStatusDTO = monitorLogStatusDataHandler.Update(monitorLogStatusDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    monitorLogStatusDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of monitor application module 
    /// </summary>
    public class MonitorLogStatusList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<MonitorLogStatusDTO> MonitorLogStatusDTOList;
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public MonitorLogStatusList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.MonitorLogStatusDTOList = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the monitor type list
        /// </summary>
        public List<MonitorLogStatusDTO> GetAllMonitorLogStatusDTO(List<KeyValuePair<MonitorLogStatusDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            MonitorLogStatusDataHandler MonitorLogStatusDataHandler = new MonitorLogStatusDataHandler();
            MonitorLogStatusDTOList = MonitorLogStatusDataHandler.GetAllMonitorLogStatusDTO(searchParameters);
            log.LogMethodExit(MonitorLogStatusDTOList);
            return MonitorLogStatusDTOList;
        }
    }
}
