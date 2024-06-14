/********************************************************************************************
* Project Name - IParafaitFiscalizationUseCases
* Description  - Interface for Fiscalization use cases.
*  
**************
**Version Log
**************
*Version     Date             Modified By          Remarks          
*********************************************************************************************
*2.155.1     11-Aug-2023       Guru S A             Created : Chile fiscalization
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Fiscalization
{
    /// <summary>
    /// IParafaitFiscalizationUseCases
    /// </summary>
    public interface IParafaitFiscalizationUseCases
    {
        /// <summary>
        /// GetPendingTransactions
        /// </summary> 
        /// <returns></returns>
        Task<List<FiscalizationPendingTransactionDTO>> GetPendingTransactions(ParafaitFiscalizationNames parafaitFiscalizationName, List<KeyValuePair<FiscalizationPendingTransactionDTO.SearchParameters, string>>
                         searchParameters, int pageNumber = 0, int pageSize = 50, SqlTransaction sqlTrx = null);
        /// <summary>
        /// GetPendingTransactionCount
        /// </summary> 
        /// <returns></returns>
        Task<int> GetPendingTransactionCount(ParafaitFiscalizationNames parafaitFiscalizationName, List<KeyValuePair<FiscalizationPendingTransactionDTO.SearchParameters, string>>
                                                       searchParameters, SqlTransaction sqlTransaction = null);
        /// <summary>
        /// PostFiscalizationReprocessingRequest
        /// </summary>
        /// <param name="fiscalizationReprocessDTOList"></param>
        /// <returns></returns>
        Task<List<FiscalizationReprocessDTO>> PostFiscalizationReprocessingRequest(ParafaitFiscalizationNames parafaitFiscalizationName, List<FiscalizationReprocessDTO> fiscalizationReprocessDTOList);
    }
}
