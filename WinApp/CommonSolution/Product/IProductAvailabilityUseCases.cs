/********************************************************************************************
* Project Name - Product
* Description  - Specification of the ProductAvailability use cases. 
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.140.00   14-Sep-2021    Roshan Devadiga        Created 
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    public interface IProductAvailabilityUseCases
    {
        Task<List<KeyValuePair<string, List<ProductsAvailabilityDTO>>>> GetProductAvailability(string loginId, bool searchUnavailableProduct = false);
        Task<List<ValidationError>> SaveAvailableProducts(List<ProductsAvailabilityDTO> productsAvailabilityDTOList, string loginId);
    }
}
