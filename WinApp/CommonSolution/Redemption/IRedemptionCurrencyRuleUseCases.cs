/********************************************************************************************
 * Project Name - RedemptionUtils
 * Description  - IRedemptionCurrencyRuleUseCase class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0          05-Nov-2020      Vikas Dwivedi             Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Semnox.Parafait.Redemption
{
    public interface IRedemptionCurrencyRuleUseCases
    {
        Task<List<RedemptionCurrencyRuleDTO>> GetRedemptionCurrencyRules(List<KeyValuePair<RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters, string>> parameters, bool loadChildRecords = false, bool activeChildRecords = true,
            int currentPage = 0, int pageSize = 0, SqlTransaction sqlTransaction = null);
        Task<string> SaveRedemptionCurrencyRules(List<RedemptionCurrencyRuleDTO> redemptionCurrencyRuleDTOList);
        Task<RedemptionCurrencyRuleContainerDTOCollection> GetRedemptionCurrencyRuleContainerDTOCollection(int siteId, string hash, bool rebuildCache);
    }
}
