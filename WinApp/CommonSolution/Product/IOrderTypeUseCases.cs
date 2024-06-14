/********************************************************************************************
 * Project Name - Products
 * Description  - IProductsUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version        Date             Modified By        Remarks          
 *********************************************************************************************
 2.130.0         19-Jul-2021      Mushahid Faizan    Created 
 ********************************************************************************************/
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    public interface IOrderTypeUseCases
    {
        Task<List<OrderTypeDTO>> GetOrderType(List<KeyValuePair<OrderTypeDTO.SearchByParameters, string>> searchParameters);
        Task<OrderTypeContainerDTOCollection> GetOrderTypeContainerDTOCollection(int siteId, string hash, bool rebuildCache);
    }
}
