/********************************************************************************************
 * Project Name - Inventory
 * Description  - IProductViewUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0     31-Dec-2020        Abhishek         Created 
 ********************************************************************************************/

using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
namespace Semnox.Parafait.Product
{
    public interface IProductActivityUseCases
    {
        Task<List<ProductActivityViewDTO>> GetProductActivities(int locationId, int productId, int lotId = -1, int currentPage = 0, int pageSize = 10, SqlTransaction sqlTransaction = null);
        Task<int> GetProductActivityCount(int locationId, int productId, SqlTransaction sqlTransaction = null);
    }
}
