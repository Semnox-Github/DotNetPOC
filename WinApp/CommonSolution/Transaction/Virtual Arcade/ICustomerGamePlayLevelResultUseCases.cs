/********************************************************************************************
* Project Name - Game
* Description  - Interface for CustomerGamePlayLevelResult Controller.
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

namespace Semnox.Parafait.Transaction.VirtualArcade
{
    /// <summary>
    /// ICustomerGamePlayLevelResultUseCases
    /// </summary>
    public interface ICustomerGamePlayLevelResultUseCases
    {

        /// <summary>
        /// GetCustomerGamePlayLevelResults
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        Task<List<CustomerGamePlayLevelResultDTO>> GetCustomerGamePlayLevelResults(List<KeyValuePair<CustomerGamePlayLevelResultDTO.SearchByParameters, string>>
                         searchParameters, SqlTransaction sqlTransaction = null);
        /// <summary>
        /// SaveCustomerGamePlayLevelResults
        /// </summary>
        /// <param name="CustomerGamePlayLevelResultDTOList"></param>
        /// <returns></returns>
        Task<List<GamePlayWinningsDTO>> SaveCustomerGamePlayLevelResults(List<CustomerGamePlayLevelResultDTO> CustomerGamePlayLevelResultDTOList);
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Task<List<GamePlayWinningsDTO>> GetCustomerGamePlayWinnings(int customerId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameMachineLevelId"></param>
        /// <returns></returns>
        Task<List<LeaderBoardDTO>> GetLeaderBoard(int gameMachineLevelId);
    }
}
