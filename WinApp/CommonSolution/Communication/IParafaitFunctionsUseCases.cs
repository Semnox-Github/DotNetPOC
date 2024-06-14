/********************************************************************************************
 * Project Name - IParafaitFunctionsUseCases
 * Description  - IParafaitFunctionsUseCases class 
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
    /// IParafaitFunctionsUseCases
    /// </summary>
    public interface IParafaitFunctionsUseCases
    {
        /// <summary>
        /// GetParafaitFunctions
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        Task<List<ParafaitFunctionsDTO>> GetParafaitFunctions(List<KeyValuePair<ParafaitFunctionsDTO.SearchByParameters, string>> searchParameters, bool loadChildren, bool loadActiveChildren);
        ///// <summary>
        ///// SaveParafaitFunctions
        ///// </summary>
        ///// <param name="productSubscriptionDTOList"></param>
        ///// <returns></returns>
        //Task<string> SaveParafaitFunctions(List<ParafaitFunctionsDTO> productSubscriptionDTOList);
    }
}
