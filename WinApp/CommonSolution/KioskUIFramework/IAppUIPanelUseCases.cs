/********************************************************************************************
* Project Name - KioskUIFramework
* Description  - Specification of the AppUIPanel . 
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
    /// 
    /// </summary>
    public interface IAppUIPanelUseCases
    {
        /// <summary>
        /// GetAppUIPanels
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="loadChildRecords"></param>
        /// <param name="activeChildRecords"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        Task<List<AppUIPanelDTO>> GetAppUIPanels(List<KeyValuePair<AppUIPanelDTO.SearchByParameters, string>> searchParameters,
                                                         bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null);
        /// <summary>
        /// SaveAppUIPanels
        /// </summary>
        /// <param name="appUIPanelsDTOList"></param>
        /// <returns></returns>
        Task<string> SaveAppUIPanels(List<AppUIPanelDTO> appUIPanelsDTOList);
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="appUIPanelsDTOList"></param>
        /// <returns></returns>
        Task<string> Delete(List<AppUIPanelDTO> appUIPanelsDTOList);

    }
}
