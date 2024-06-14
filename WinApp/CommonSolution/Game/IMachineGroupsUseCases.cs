/********************************************************************************************
* Project Name - Game
* Description  - Interface for GenericCalendar Controller.
*  
**************
**Version Log
**************
*Version     Date             Modified By          Remarks          
*********************************************************************************************
*2.110.0     16-Dec-2020     Prajwal S             Created : Web Inventory UI Redesign with REST API
*2.110.0     05-Feb-2021     Fiona                 Modified for pagination and to Get Machine Groups Count
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Semnox.Parafait.Game
{
    public interface IMachineGroupsUseCases
    {
        Task<List<MachineGroupsDTO>> GetMachineGroups(List<KeyValuePair<MachineGroupsDTO.SearchByParameters, string>> searchParameters, bool loadActiveChild = false, bool buildChildRecords = false,int currentPage=0,int pageSize=0, SqlTransaction sqlTransaction = null);
        Task<string> SaveMachineGroups(List<MachineGroupsDTO> machineInputDeviceDTOList);
        Task<int> GetMachineGroupsCount(List<KeyValuePair<MachineGroupsDTO.SearchByParameters, string>>
                                                       searchParameters, SqlTransaction sqlTransaction = null);
    }
}
