/********************************************************************************************
 * Project Name -Monitor
 * Description  - IMonitorUseCases class
 *
 **************
 ** Version Log
  **************
  * Version      Date              Modified By         Remarks
 *********************************************************************************************
 2.120.0         10-May-2021       B Mahesh Pai       Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.logger
{
   public interface IMonitorUseCases
    {
        Task<List<MonitorDTO>>GetMonitors (List<KeyValuePair<MonitorDTO.SearchByParameters, string>> searchParameters, int currentPage, int pageSize, bool loadChildRecords = false, bool loadActiveRecords = false,
                                           SqlTransaction sqlTransaction = null);
        Task<string> SaveMonitors(List<MonitorDTO> monitorDTOList);
        Task<string> Delete(List<MonitorDTO> monitorDTOList);

    }
}
