/********************************************************************************************
 * Project Name - Game  
 * Description  - IMachineDataService class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Aug-2020       Girish Kundar             Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;

namespace Semnox.Parafait.Game
{
    public interface IMachineDataService
    {
        List<MachineDTO> GetMachines(List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>> parameters, bool loadChildRecords = false);
        string PostMachines(List<MachineDTO> machineDTOList);
        string DeleteMachines(List<MachineDTO> machineDTOList);
    }
}
