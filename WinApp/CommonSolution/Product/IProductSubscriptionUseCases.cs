/********************************************************************************************
 * Project Name - IProductSubscriptionUseCases
 * Description  - IProductSubscriptionUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0       25-Jan-2021      Guru S A                Created for subcription changes
 ********************************************************************************************/
using System.Collections.Generic; 
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// IProductSubscriptionUseCases
    /// </summary>
    public interface IProductSubscriptionUseCases
    {
        /// <summary>
        /// GetProductSubscription
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        Task<List<ProductSubscriptionDTO>> GetProductSubscription(List<KeyValuePair<ProductSubscriptionDTO.SearchByParameters, string>> searchParameters);
        /// <summary>
        /// SaveProductSubscription
        /// </summary>
        /// <param name="productSubscriptionDTOList"></param>
        /// <returns></returns>
        Task<string> SaveProductSubscription(List<ProductSubscriptionDTO> productSubscriptionDTOList);
    }
}
