/********************************************************************************************
* Project Name - Product
* Description  - Specification of the MasterSchedule use cases. 
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.120.00   10-Mar-2021   Roshan Devadiga        Created 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    public interface IMasterScheduleUseCases
    {
        Task<List<MasterScheduleDTO>> GetMasterSchedules(List<KeyValuePair<MasterScheduleDTO.SearchByParameters, string>> searchParameters,
               bool loadChildActiveRecords = false, bool loadChildRecord = false, int facilityMapId = -1, SqlTransaction sqlTransaction = null);
        Task<string> SaveMasterSchedules(List<MasterScheduleDTO> masterScheduleDTOList);
        Task<string> Delete(List<MasterScheduleDTO> masterScheduleDTOList);
    }
}
