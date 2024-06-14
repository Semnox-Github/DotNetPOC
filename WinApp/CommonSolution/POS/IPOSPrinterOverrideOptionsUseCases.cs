/********************************************************************************************
 * Project Name - POS
 * Description  - IPosPrinterOverrideRulesUsecases class to get the data  from API by doing remote call  
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
    /// IPOSPrinterOverrideOptionsUseCases
    /// </summary>
    public interface IPOSPrinterOverrideOptionsUseCases
    {
        /// <summary>
        /// GetPOSPrinterOverrideOptions
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        Task<List<POSPrinterOverrideOptionsDTO>> GetPOSPrinterOverrideOptions(List<KeyValuePair<POSPrinterOverrideOptionsDTO.SearchByParameters, string>> parameters, SqlTransaction sqlTransaction = null);
        /// <summary>
        /// SavePOSPrinterOverrideOptions
        /// </summary>
        /// <param name="pOSPrinterOverrideOptionsDTOList"></param>
        /// <returns></returns>
        Task<string> SavePOSPrinterOverrideOptions(List<POSPrinterOverrideOptionsDTO> pOSPrinterOverrideOptionsDTOList);
    }
}
