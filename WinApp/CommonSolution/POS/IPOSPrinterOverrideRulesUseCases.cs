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
    public interface IPOSPrinterOverrideRulesUseCases
    {
        Task<List<POSPrinterOverrideRulesDTO>> GetPOSPrinterOverrideRules(List<KeyValuePair<POSPrinterOverrideRulesDTO.SearchByParameters, string>> parameters, SqlTransaction sqlTransaction = null);
        Task<string> SavePOSPrinterOverrideRules(List<POSPrinterOverrideRulesDTO> pOSPrinterOverrideRulesDTOList);
    }
}
