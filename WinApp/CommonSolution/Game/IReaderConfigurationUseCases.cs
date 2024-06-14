/********************************************************************************************
* Project Name - Game
* Description  - Interface for ReaderConfigutation Controller.
*  
**************
**Version Log
**************
*Version     Date             Modified By          Remarks          
*********************************************************************************************
*2.110.0     11-Dec-2020     Prajwal S             Created : Web Inventory UI Redesign with REST API
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Semnox.Parafait.Game
{
    public interface IReaderConfigurationUseCases
    {
        Task<List<MachineAttributeDTO>> GetMachineAttributes(List<KeyValuePair<string, string>> searchByParameters, SqlTransaction sqlTransaction = null);
        Task<string> SaveMachineAttributes(List<MachineAttributeDTO> MachineAttributeDTOList, string moduleName, string moduleId);
        Task<ReaderConfigurationContainerDTOCollection> GetMachineAttributeContainerDTOCollection(int siteId, string hash, bool rebuildCache );
        Task<MachineAttributeDTO> DeleteMachineAttributes(MachineAttributeDTO machineAttributeDTO, string entityName, string entityId);
    }
}
