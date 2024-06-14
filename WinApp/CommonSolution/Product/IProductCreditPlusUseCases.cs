/********************************************************************************************
 * Project Name - Product
 * Description  - IProductCreditPlusUseCases Interface
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
*2.140.00   14-Sep-2021       B Mahesh Pai        Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    public interface IProductCreditPlusUseCases
    {
        Task<List<ProductCreditPlusDTO>> GetProductCreditPlus(List<KeyValuePair<ProductCreditPlusDTO.SearchByParameters, string>> parameters);
        Task<string> SaveProductCreditPlus(List<ProductCreditPlusDTO> ProductCreditPlusDTOList);
    }
}
