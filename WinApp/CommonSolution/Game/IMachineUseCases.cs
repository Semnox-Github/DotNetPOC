/********************************************************************************************
* Project Name - Game
* Description  - Interface for Machine Controller.
*  
**************
**Version Log
**************
*Version     Date             Modified By          Remarks          
*********************************************************************************************
*2.110.0     14-Dec-2020     Prajwal S             Created : Web Inventory UI Redesign with REST API
*2.110.0     04-Feb-2021     Prajwal S             Modified to get Machine Count
*2.130.0    06-Aug-2021      Abhishek              Modified to get Machine attribute use case
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using static Semnox.Parafait.Game.MachineConfigurationClass;

namespace Semnox.Parafait.Game
{
    public interface IMachineUseCases
    {
        Task<List<MachineDTO>> GetMachines(List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>>
                          searchParameters, bool loadAttributes = false, int currentPage = 0, int pageSize = 0, SqlTransaction sqlTransaction = null
                          );
        Task<string> SaveMachines(List<MachineDTO> MachineDTOList);
        Task<MachineContainerDTOCollection> GetMachineContainerDTOCollection(int siteId, string hash, bool rebuildCache);
        Task<string> DeleteMachines(List<MachineDTO> MachineDTOList);

        Task<int> GetMachineCount(List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>>
                                                       searchParameters, SqlTransaction sqlTransaction = null);
        Task<List<MachineAttributeLogDTO>> GetMachineAttributeLogs(List<KeyValuePair<MachineAttributeLogDTO.SearchByParameters, string>>
                  searchParameters);
        //Task<MachineDTO> SetMachineQRCode(int machineId, string qrCodeString);

        Task<List<clsConfig>> GetMachineConfiguration(int machineId, int promotionDetailId);
    }
}

