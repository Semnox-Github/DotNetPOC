/********************************************************************************************
* Project Name - Game
* Description  - Interface for GameMachineLevel Controller.
*  
**************
**Version Log
**************
*Version     Date             Modified By          Remarks          
*********************************************************************************************
*2.110.0     08-Feb-2021       Fiona               Created
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Semnox.Parafait.Game.VirtualArcade
{
    /// <summary>
    /// IGameMachineLevelUseCases
    /// </summary>
    public interface IGameMachineLevelUseCases
    {
        /// <summary>
        /// GetGameMachineLevels
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        Task<List<GameMachineLevelDTO>> GetGameMachineLevels(List<KeyValuePair<GameMachineLevelDTO.SearchByParameters, string>>
                         searchParameters,  SqlTransaction sqlTransaction = null);
        /// <summary>
        /// SaveGameMachineLevels
        /// </summary>
        /// <param name="GameMachineLevelDTOList"></param>
        /// <returns></returns>
        Task<List<GameMachineLevelDTO>> SaveGameMachineLevels(List<GameMachineLevelDTO> GameMachineLevelDTOList);
       
    }
}
