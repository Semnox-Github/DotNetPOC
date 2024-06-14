/********************************************************************************************
 * Project Name -MonitorAsset
 * Description  -IMonitorAssetUseCases class
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
   public interface IMonitorAssetUseCases
    {
        Task<List<MonitorAssetDTO>> GetMonitorAssets(List<KeyValuePair<MonitorAssetDTO.SearchByMonitorAssetParameters, string>> searchParameters, SqlTransaction sqlTransaction = null);
        Task<string> SaveMonitorAssets(List<MonitorAssetDTO> monitorAssetDTOList);
        Task<string> Delete(List<MonitorAssetDTO> monitorAssetDTOList);
    }
}
