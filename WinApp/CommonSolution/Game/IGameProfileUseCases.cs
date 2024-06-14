/********************************************************************************************
* Project Name - Game
* Description  - Interface for GameProfile Controller.
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
        public interface IGameProfileUseCases
    {
        Task<List<GameProfileDTO>> GetGameProfiles(List<KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>>
                          searchParameters, bool loadChildRecords = false,  int currentPage = 0, int pageSize = 0, SqlTransaction sqlTransaction = null,
                          bool activeChildRecords = true);
        Task<List<GameProfileDTO>> SaveGameProfiles(List<GameProfileDTO> GameProfileDTOList);
        Task<GameProfileContainerDTOCollection> GetGameProfileContainerDTOCollection(int siteId, string hash, bool rebuildCache);
        Task<string> DeleteGameProfiles(List<GameProfileDTO> GameProfileDTOList);

        Task<int> GetGameProfileCount(List<KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>>
                                                     searchParameters, SqlTransaction sqlTransaction = null);
    }

}
