/********************************************************************************************
 * Project Name - IParafaitFunctionEventUseCases
 * Description  - IParafaitFunctionEventUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0       10-Feb-2021      Guru S A                Created for subcription changes
 ********************************************************************************************/
using System.Collections.Generic; 
using System.Threading.Tasks;

namespace Semnox.Parafait.Communication
{
    /// <summary>
    /// IParafaitFunctionEventUseCases
    /// </summary>
    public interface IParafaitFunctionEventUseCases
    {
        /// <summary>
        /// GetParafaitFunctionEvent
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        Task<List<ParafaitFunctionEventDTO>> GetParafaitFunctionEvent(List<KeyValuePair<ParafaitFunctionEventDTO.SearchByParameters, string>> searchParameters);
        ///// <summary>
        ///// SaveParafaitFunctionEvent
        ///// </summary>
        ///// <param name="productSubscriptionDTOList"></param>
        ///// <returns></returns>
        //Task<string> SaveParafaitFunctionEvent(List<ParafaitFunctionEventDTO> productSubscriptionDTOList);
    }
}
