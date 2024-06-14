/********************************************************************************************
 * Project Name - POS
 * Description  - IOverrideOpitonItemUsecase class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
  2.110       29-Dec-2020      Dakshakh Raj              Created : Peru Invoice changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.POS
{
    /// <summary>
    /// IOverrideItemUseCases
    /// </summary>
    public interface IOverrideItemUseCases
    {
        /// <summary>
        /// GetOverrideOptionItems
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        Task<List<OverrideOptionItemDTO>> GetOverrideOptionItems(List<KeyValuePair<OverrideOptionItemDTO.SearchByParameters, string>> parameters, SqlTransaction sqlTransaction = null);
    }
}
