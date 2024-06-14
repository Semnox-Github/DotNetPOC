/********************************************************************************************
* Project Name - Product
* Description  - Specification of the ProductDiscount use cases. 
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.120.00   30-Mar-2021   Roshan Devadiga        Created 
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
   public interface IProductDiscountUseCases
    {
        Task<List<ProductDiscountsDTO>> GetProductDiscounts(List<KeyValuePair<ProductDiscountsDTO.SearchByParameters, string>> parameters);
        Task<string> SaveProductDiscounts(List<ProductDiscountsDTO> productDiscountsDTOList);
        Task<string> Delete(List<ProductDiscountsDTO> productDiscountsDTOList);
    }
}
