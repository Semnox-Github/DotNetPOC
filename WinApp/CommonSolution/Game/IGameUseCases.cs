/********************************************************************************************
* Project Name - Game
* Description  - Interface for Game Controller.
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
    public interface IGameUseCases
    {
        Task<List<GameDTO>> GetGames(List<KeyValuePair<GameDTO.SearchByGameParameters, string>>
                          searchParameters, bool loadChildRecords = false, int currentPage = 0, int pageSize = 0,
                          bool activateChildRecords = false);
        Task<List<GameDTO>> SaveGames(List<GameDTO> GameDTOList);
        Task<GameContainerDTOCollection> GetGameContainerDTOCollection(int siteId, string hash, bool rebuildCache);
        Task<string> DeleteGames(List<GameDTO> GameDTOList);
        Task<int> GetGameCount(List<KeyValuePair<GameDTO.SearchByGameParameters, string>>
                                                     searchParameters, SqlTransaction sqlTransaction = null);
        Task<List<AllowedMachineNamesDTO>> GetAllowedMachineNames(int allowedMachineId=-1,int gameId=-1,string machineName=null,string isActive=null,int siteId=-1);
        Task<string> SaveAllowedMachineNames(int gameId, List<AllowedMachineNamesDTO> allowedMachineNamesDTOList);
    }

}