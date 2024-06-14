/********************************************************************************************
 * Project Name - MonitorLog
 * Description  - Remote proxy class to the MonitorLog use cases.  
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By              Remarks          
 *********************************************************************************************
 2.150.0      16-Mar-2022       Prajwal S                Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.logger
{
    /// <summary>
    /// Remote proxy class to the monitorLog use cases.
    /// </summary>
    public class RemoteMonitorLogUseCases : RemoteUseCases, IMonitorLogUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string MONITOR_LOG_URL = "api/Log/MonitorLog";
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        public RemoteMonitorLogUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }


        public async Task<List<MonitorLogDTO>> GetMonitorLogs(int monitorLogId = -1, int monitorId = -1, bool isActive = false)
        {
            List<MonitorLogDTO> result = await Get<List<MonitorLogDTO>>(MONITOR_LOG_URL,
                                                                        new WebApiGetRequestParameterCollection("monitorLogId",
                                                                                                                monitorLogId,
                                                                                                                "monitorId",
                                                                                                                monitorId,
                                                                                                                "isActive",
                                                                                                                isActive));
            log.LogMethodExit(result);
            return result;
        }

        public async Task<MonitorLogDTO> SaveMonitorLogs(LogMonitorDTO logMonitorDTO)
        {
            log.LogMethodEntry(logMonitorDTO);
            MonitorLogDTO result = await Post<MonitorLogDTO>(MONITOR_LOG_URL, logMonitorDTO);
            log.LogMethodExit(result);
            return result;
        }
    }
}
