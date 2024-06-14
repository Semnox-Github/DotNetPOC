/********************************************************************************************
* Project Name - Game
* Description  - Interface for GamePlay Controller.
*  
**************
**Version Log
**************
*Version     Date             Modified By          Remarks          
*********************************************************************************************
*2.110.0     08-Dec-2020     Prajwal S             Created : Web Inventory UI Redesign with REST API
*2.110.0     04-Feb-2021     Fiona                 Modified by adding GetGamePlayCount()
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Semnox.Parafait.Game
{
    public interface IGamePlayUseCases
    {
        Task<List<GamePlayDTO>> GetGamePlays(List<KeyValuePair<GamePlayDTO.SearchByParameters, string>>
                         searchParameters, bool loadChildRecords, int currentPage = 0, int pageSize = 0, SqlTransaction sqlTransaction = null);
        Task<string> SaveGamePlays(List<GamePlayDTO> GamePlayDTOList);
        Task<int> GetGamePlayCount(List<KeyValuePair<GamePlayDTO.SearchByParameters, string>>
                                                       searchParameters, SqlTransaction sqlTransaction = null);
    }
}
