/********************************************************************************************
 * Project Name - POS
 * Description  - ITrxPOSPrinterOverrideRulesUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
  2.110       06-Jan-2021      Dakshakh Raj              Created : Peru Invoice changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// ITrxPOSPrinterOverrideRulesUseCases
    /// </summary>
    public interface ITrxPOSPrinterOverrideRulesUseCases
    {
        /// <summary>
        /// Get Trx POS Printer Override Rules
        /// </summary>
        /// <param name="parameters">parameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        Task<List<TrxPOSPrinterOverrideRulesDTO>> GetTrxPOSPrinterOverrideRules(List<KeyValuePair<TrxPOSPrinterOverrideRulesDTO.SearchByParameters, string>> parameters, SqlTransaction sqlTransaction = null);

        /// <summary>
        /// Save Trx POS Printer Override Rules
        /// </summary>
        /// <param name="trxPOSPrinterOverrideRulesDTOList">trxPOSPrinterOverrideRulesDTOList</param>
        /// <returns></returns>
        Task<string> SaveTrxPOSPrinterOverrideRules(List<TrxPOSPrinterOverrideRulesDTO> trxPOSPrinterOverrideRulesDTOList);
    }
}
