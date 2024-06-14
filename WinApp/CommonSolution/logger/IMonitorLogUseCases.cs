/********************************************************************************************
* Project Name - Logger
* Description  - Specification of Monitor Log use cases.
*  
**************
**Version Log
**************
*Version     Date             Modified By          Remarks          
*********************************************************************************************
*2.150.0     16-Mar-2022     Prajwal S             Created
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Semnox.Parafait.logger
{
    public interface IMonitorLogUseCases
    {
        Task<List<MonitorLogDTO>> GetMonitorLogs(int monitorLogId = -1, int monitorId = -1, bool isActive = false);
        Task<MonitorLogDTO> SaveMonitorLogs(LogMonitorDTO logMonitorDTO);
    }
}