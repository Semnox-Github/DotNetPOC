/********************************************************************************************
* Project Name - Products
* Description  - IProductsDisplayGroupUseCases  class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.120.00    06-Apr-2021       B Mahesh Pai        Created : POS UI Redesign with REST API
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    public interface IProductDisplayGroupUseCases
    {
        Task<List<ProductsDisplayGroupDTO>> GetProductsDisplayGroups(List<KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>> searchParameters, SqlTransaction sqlTransaction = null);
        Task<string> SaveProductsDisplayGroups(List<ProductsDisplayGroupDTO> productsDisplayGroupDTOList);
        Task<string> Delete(List<ProductsDisplayGroupDTO> productsDisplayGroupDTOList);
    }
}
