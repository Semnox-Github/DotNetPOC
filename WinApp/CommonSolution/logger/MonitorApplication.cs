/********************************************************************************************
 * Project Name - Monitor Application
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
    /// Monitor Applications
    /// </summary>
    public class MonitorApplication
    {
        private MonitorApplicationDTO monitorApplicationDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        private MonitorApplication(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="monitorApplicationDTO"></param>
        public MonitorApplication(ExecutionContext executionContext, MonitorApplicationDTO monitorApplicationDTO)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext, monitorApplicationDTO);
            this.monitorApplicationDTO = monitorApplicationDTO;
            log.LogMethodExit();
        }
        /// <summary>
        /// Saves the monitor application
        /// Monitor application will be inserted if applicationId is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save()
        {
            log.LogMethodEntry();
            MonitorApplicationDataHandler monitorApplicationDataHandler = new MonitorApplicationDataHandler();
            if (monitorApplicationDTO.ApplicationId <= 0)
            {
                monitorApplicationDTO = monitorApplicationDataHandler.Insert(monitorApplicationDTO, executionContext.GetUserId(), executionContext.GetSiteId());                
            }
            else
            {
                if (monitorApplicationDTO.IsChanged == true)
                {
                    monitorApplicationDTO = monitorApplicationDataHandler.Update(monitorApplicationDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    monitorApplicationDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of monitor application 
    /// </summary>
    public class MonitorApplicationList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<MonitorApplicationDTO> monitorApplicationDTOList;
        private ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public MonitorApplicationList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.monitorApplicationDTOList = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the monitor application list
        /// </summary>
        public List<MonitorApplicationDTO> GetAllMonitorApplicationDTO(List<KeyValuePair<MonitorApplicationDTO.SearchByMonitorApplicationParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            MonitorApplicationDataHandler monitorApplicationDataHandler = new MonitorApplicationDataHandler();            
            monitorApplicationDTOList = monitorApplicationDataHandler.GetAllMonitorApplicationDTO(searchParameters);
            log.LogMethodExit(monitorApplicationDTOList);
            return monitorApplicationDTOList;
        }
    }
}
