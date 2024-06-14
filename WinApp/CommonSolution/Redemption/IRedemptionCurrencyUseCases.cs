/********************************************************************************************
 * Project Name - RedemptionUtils
 * Description  - IRedemptionCurrencyUseCase class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0          05-Nov-2020      Vikas Dwivedi             Created : POS UI Redesign with REST API
 2.110.1      17-Feb-2021      Mushahid Faizan           Modified - Web Inventory Phase 2 changes
 ********************************************************************************************/
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Semnox.Parafait.Redemption
{
    public interface IRedemptionCurrencyUseCases
    {
        Task<List<RedemptionCurrencyDTO>> GetRedemptionCurrencies(List<KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>> parameters, int currentPage = 0, int pageSize = 0, SqlTransaction sqlTransaction = null);
        Task<string> SaveRedemptionCurrencies(List<RedemptionCurrencyDTO> redemptionCurrencyDTOList);
        Task<int> GetRedemptionCurrencyCount(List<KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>> parameters);
        Task<RedemptionCurrencyContainerDTOCollection> GetRedemptionCurrencyContainerDTOCollection(int siteId, string hash, bool rebuildCache);
       
    }
}
