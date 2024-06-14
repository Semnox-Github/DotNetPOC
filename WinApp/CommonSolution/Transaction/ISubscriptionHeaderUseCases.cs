/********************************************************************************************
 * Project Name - ISubscriptionHeaderUseCases
 * Description  - ISubscriptionHeaderUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0       25-Jan-2021      Guru S A                Created for subcription changes
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;
using System.Collections.Generic; 
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// ISubscriptionHeaderUseCases
    /// </summary>
    public interface ISubscriptionHeaderUseCases
    {
        /// <summary>
        /// GetSubscriptionHeader.
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="utilities"></param>
        /// <param name="loadChildren"></param>
        /// <returns></returns>
        Task<List<SubscriptionHeaderDTO>> GetSubscriptionHeader(List<KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>> searchParameters, Utilities utilities, bool loadChildren);
        /// <summary>
        /// SaveSubscriptionHeader
        /// </summary>
        /// <param name="subscriptionHeaderDTOList"></param>
        /// <returns></returns>
        Task<string> SaveSubscriptionHeader(List<SubscriptionHeaderDTO> subscriptionHeaderDTOList);
    }
}
