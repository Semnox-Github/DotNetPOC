/********************************************************************************************
 * Project Name - Inventory
 * Description  - ITaxUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0         09-Nov-2020       Mushahid Faizan         Created 
 ********************************************************************************************/

using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    public interface ITaxUseCases
    {
        Task<List<TaxDTO>> GetTaxes(List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>>
               searchParameters, bool buildChildRecords, bool loadActiveChild, SqlTransaction sqlTransaction = null, int currentPage = 0, int pageSize = 0);
        Task<string> SaveTaxes(List<TaxDTO> locationDTOList);
        Task<string> DeleteTaxes(List<TaxDTO> locationDTOList);
    }
}
