/********************************************************************************************
* Project Name - Product
* Description  - Specification of the ProductsSpecialPricing use cases. 
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.120.00   09-Mar-2021   Roshan Devadiga        Created 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    public interface IProductsSpecialPricingUseCases
    {
        Task<List<ProductsSpecialPricingDTO>> GetProductsSpecialPricings(List<KeyValuePair<ProductsSpecialPricingDTO.SearchByProductsSpecialPricingParameters, string>> searchParameters);
        Task<string> SaveProductsSpecialPricings(List<ProductsSpecialPricingDTO> productsSpecialPricingList);
        Task<string> Delete(List<ProductsSpecialPricingDTO> productsSpecialPricingList);
    }
}
