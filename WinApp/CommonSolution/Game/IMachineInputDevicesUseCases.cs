/********************************************************************************************
* Project Name - Game
* Description  - Interface for MachineInputDevice Controller.
*  
**************
**Version Log
**************
*Version     Date             Modified By          Remarks          
*********************************************************************************************
*2.110.0     08-Dec-2020     Prajwal S             Created : Web Inventory UI Redesign with REST API
*2.110.0     05-Feb-2021     Fiona                  Modified to get Input Devices Count
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Semnox.Parafait.Game
{
    public interface IMachineInputDevicesUseCases
    { 
        Task<List<MachineInputDevicesDTO>> GetMachineInputDevices(List<KeyValuePair<MachineInputDevicesDTO.SearchByParameters, string>> searchParameters, int currentPage = 0, int pageSize = 0, SqlTransaction sqlTransaction = null );
        Task<string> SaveMachineInputDevices(List<MachineInputDevicesDTO> MachineInputDeviceDTOList);
        Task<int> GetMachineInputDevicesCount(List<KeyValuePair<MachineInputDevicesDTO.SearchByParameters, string>>
                                                       searchParameters, SqlTransaction sqlTransaction = null);

    }
}
