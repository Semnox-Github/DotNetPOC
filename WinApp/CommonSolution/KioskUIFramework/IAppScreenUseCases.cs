/********************************************************************************************
* Project Name - KioskUIFramework
* Description  - Specification of the AppScreen . 
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.120.00   27-Apr-2021   Roshan Devadiga        Created 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.KioskUIFamework
{
    /// <summary>
    /// IAppScreenUseCases
    /// </summary>
    public interface IAppScreenUseCases
    {
        /// <summary>
        /// GetAppScreens
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="loadChildRecords"></param>
        /// <param name="activeChildRecords"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        Task<List<AppScreenDTO>> GetAppScreens(List<KeyValuePair<AppScreenDTO.SearchByParameters, string>> searchParameters,
                                                         bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null);
        /// <summary>
        /// SaveAppScreens
        /// </summary>
        /// <param name="appScreenDTOList"></param>
        /// <returns></returns>
        Task<string> SaveAppScreens(List<AppScreenDTO> appScreenDTOList);
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="appScreenDTOList"></param>
        /// <returns></returns>
        Task<string> Delete(List<AppScreenDTO> appScreenDTOList);
    }
}
