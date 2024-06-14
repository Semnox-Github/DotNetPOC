/********************************************************************************************
* Project Name - Product
* Description  - Specification of the SaleGroupProductMap use cases. 
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.120.00   19-Apr-2021   Roshan Devadiga        Created 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
  public interface ISaleGroupProductMapUseCases
    {
        Task<List<SaleGroupProductMapDTO>> GetSaleGroupProductMaps(List<KeyValuePair<SaleGroupProductMapDTO.SearchBySaleGroupProductMapParameters, string>> searchParameters);
        Task<string> SaveSaleGroupProductMaps(List<SaleGroupProductMapDTO> saleGroupProductMapList);

    }
}
