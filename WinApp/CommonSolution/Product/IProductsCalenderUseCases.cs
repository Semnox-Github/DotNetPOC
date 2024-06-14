/********************************************************************************************
* Project Name - Product
* Description  - Specification of the ProductsCalender use cases. 
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.120.00   08-Mar-2021   Roshan Devadiga        Created 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    public interface  IProductsCalenderUseCases
    {
        Task<List<ProductsCalenderDTO>> GetProductsCalenders(List<KeyValuePair<ProductsCalenderDTO.SearchByParameters, string>> searchParameters);
        Task<string> SaveProductsCalenders(List<ProductsCalenderDTO> productsCalenderList);
        Task<string> Delete(List<ProductsCalenderDTO> productsCalenderDTOList);
    }
}
