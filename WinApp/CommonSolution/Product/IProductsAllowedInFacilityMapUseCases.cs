/********************************************************************************************
* Project Name - User
* Description  - IProductsAllowedInFacilityMap  class 
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
    public interface IProductsAllowedInFacilityMapUseCases
    {
        Task<List<ProductsAllowedInFacilityMapDTO>> GetProductsAllowedInFacilityMaps(List<KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>> searchParameters, bool loadProductsDTO = false, SqlTransaction sqlTransaction = null);
        Task<string> SaveProductsAllowedInFacilityMaps(List<ProductsAllowedInFacilityMapDTO> productsAllowedInFacilityMapDTO);
    }
}
