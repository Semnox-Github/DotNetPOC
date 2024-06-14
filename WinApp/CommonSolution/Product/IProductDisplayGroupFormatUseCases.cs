/********************************************************************************************
* Project Name - Product
* Description  - Specification of the ProductDisplayGroupFormat use cases. 
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.140.00   14-Sep-2021    Roshan Devadiga        Created 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
   public interface IProductDisplayGroupFormatUseCases
    {
        Task<List<ProductDisplayGroupFormatDTO>> GetProductDisplayGroupFormats(List<KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>> parameters, bool loadChildRecords = false, bool loadActiveChildRecords = false,SqlTransaction sqlTransaction = null);
        Task<string> SaveProductDisplayGroupFormats(List<ProductDisplayGroupFormatDTO> productDisplayGroupFormatDTOList);
        Task<string> Delete(List<ProductDisplayGroupFormatDTO> productDisplayGroupFormatDTOList);
        Task<List<ProductDisplayGroupFormatDTO>> GetConfiguredDisplayGroupListForLogin(string loginId, SqlTransaction sqlTransaction = null);
        Task<ProductDisplayGroupFormatContainerDTOCollection> GetProductDisplayGroupFormatContainerDTOCollection(int siteId, string hash, bool rebuildCache);

    }
}
