/********************************************************************************************
* Project Name - Game
* Description  - Interface for Hub Controller.
*  
**************
**Version Log
**************
*Version     Date             Modified By          Remarks          
*********************************************************************************************
*2.110.0     08-Dec-2020     Prajwal S             Created : Web Inventory UI Redesign with REST API
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Semnox.Parafait.Game
{
   public interface IHubUseCases
    {
        Task<List<HubDTO>> GetHubs(List<KeyValuePair<HubDTO.SearchByHubParameters, string>>
                          searchParameters, bool loadMachineCount = false, bool loadChild = false, int currentPage = 0, int pageSize = 0, SqlTransaction sqlTransaction = null
                          );
        Task<string> SaveHubs(List<HubDTO> HubDTOList);
        Task<HubContainerDTOCollection> GetHubContainerDTOCollection(int siteId, string hash, bool rebuildCache);
        Task<string> DeleteHubs(List<HubDTO> HubDTOList);
        Task<int> GetHubCount(List<KeyValuePair<HubDTO.SearchByHubParameters, string>>
                                                       searchParameters, SqlTransaction sqlTransaction = null);
        /// <summary>
        /// SaveHubStatus
        /// </summary>
        /// <param name="hubId">hubId</param>
        /// <param name="hubStatusDTO">hubStatusDTO</param>
        Task<HubDTO> SaveHubStatus(int hubId, HubStatusDTO hubStatusDTO);
   }
}
