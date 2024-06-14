/********************************************************************************************
* Project Name - Product
* Description  - Specification of the CheckOutPrices use cases. 
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    public interface ICheckOutPricesUseCases
    {
        Task<List<CheckOutPricesDTO>> GetCheckOutPrices(List<KeyValuePair<CheckOutPricesDTO.SearchByParameters, string>> searchParameters);
        Task<string> SaveCheckOutPrices(List<CheckOutPricesDTO> checkOutPricesDTOList);
        Task<string> Delete(List<CheckOutPricesDTO> checkOutPricesDTOList);
    }
}
