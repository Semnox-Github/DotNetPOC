/********************************************************************************************
 * Project Name -MonitorPriority
 * Description  - IMonitorPriorityUseCases class
 *
 **************
 ** Version Log
  **************
  * Version      Date              Modified By         Remarks
 *********************************************************************************************
 2.120.0         11-May-2021       B Mahesh Pai       Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.logger
{
    public interface IMonitorPriorityUseCases
    {
        Task<List<MonitorPriorityDTO>> GetMonitorPriorities(List<KeyValuePair<MonitorPriorityDTO.SearchByParameters, string>> searchParameters);
        Task<string> SaveMonitorPriorities(List<MonitorPriorityDTO> monitorAssetDTOList);
        Task<string> Delete(List<MonitorPriorityDTO> monitorAssetDTOList);
    }
}
