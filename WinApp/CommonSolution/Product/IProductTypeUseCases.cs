/********************************************************************************************
 * Project Name - Product  
 * Description  - IProductTypeUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.1    24-Jun-2021         Abhishek           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    public interface IProductTypeUseCases
    {
        Task<List<ProductTypeDTO>> GetProductType(List<KeyValuePair<ProductTypeDTO.SearchByParameters, string>>
                       searchParameters, SqlTransaction sqlTransaction = null
                       );
        Task<ProductTypeContainerDTOCollection> GetProductTypeContainerDTOCollection(int siteId, string hash, bool rebuildCache);
    }
}
