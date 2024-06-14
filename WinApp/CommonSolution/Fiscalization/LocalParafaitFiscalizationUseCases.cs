/********************************************************************************************
 * Project Name - Fiscalization
 * Description  - LocalParafaitFiscalizationUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.155.1      11-Aug-2023     Guru S A                Created for chile fiscalization
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Fiscalization
{
    /// <summary>
    /// LocalParafaitFiscalizationUseCases
    /// </summary>
    public class LocalParafaitFiscalizationUseCases : IParafaitFiscalizationUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private Utilities utilities;
        /// <summary>
        /// LocalParafaitFiscalizationUseCases
        /// </summary> 
        public LocalParafaitFiscalizationUseCases(ExecutionContext executionContext, Utilities utilities)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.utilities = utilities;
            log.LogMethodExit();
        }
        /// <summary>
        /// GetPendingTransactions
        /// </summary> 
        /// <returns></returns>
        public async Task<List<FiscalizationPendingTransactionDTO>> GetPendingTransactions(ParafaitFiscalizationNames parafaitFiscalizationName, List<KeyValuePair<FiscalizationPendingTransactionDTO.SearchParameters, string>>
                         searchParameters, int pageNumber = 0, int pageSize = 50, SqlTransaction sqlTrx = null)
        {
            return await Task<List<FiscalizationPendingTransactionDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(parafaitFiscalizationName, searchParameters, pageNumber, pageSize, sqlTrx);
                ParafaitFiscalizationList parafaitFiscalizationList = FiscalizationFactory.GetParafaitFiscalizationList(parafaitFiscalizationName, executionContext, utilities);
                List<FiscalizationPendingTransactionDTO> list = parafaitFiscalizationList.GetPendingTransactions(searchParameters, pageNumber, pageSize, sqlTrx);
                log.LogMethodExit(list);
                return list;
            });
        }
        /// <summary>
        /// GetPendingTransactionCount
        /// </summary> 
        /// <returns></returns>
        public async Task<int> GetPendingTransactionCount(ParafaitFiscalizationNames parafaitFiscalizationName, List<KeyValuePair<FiscalizationPendingTransactionDTO.SearchParameters, string>> searchParameters, SqlTransaction sqlTrx = null)
        {
            return await Task<int>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(parafaitFiscalizationName, searchParameters, sqlTrx);
                ParafaitFiscalizationList parafaitFiscalizationList = FiscalizationFactory.GetParafaitFiscalizationList(parafaitFiscalizationName, executionContext, utilities);
                int count = parafaitFiscalizationList.GetPendingTransactionCount(searchParameters, sqlTrx);

                log.LogMethodExit(count);
                return count;
            });
        }         
        /// <summary>
        /// PostFiscalizationReprocessingRequest
        /// </summary>
        /// <param name="fiscalizationReprocessDTOList"></param>
        /// <returns></returns>
        public async Task<List<FiscalizationReprocessDTO>> PostFiscalizationReprocessingRequest(ParafaitFiscalizationNames parafaitFiscalizationName, List<FiscalizationReprocessDTO> fiscalizationReprocessDTOList)
        {
            return await Task<List<FiscalizationReprocessDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(parafaitFiscalizationName, fiscalizationReprocessDTOList);
                ParafaitFiscalizationList parafaitFiscalizationList = FiscalizationFactory.GetParafaitFiscalizationList(parafaitFiscalizationName, executionContext, utilities);
                List<FiscalizationReprocessDTO> dtoList = parafaitFiscalizationList.PostFiscalizationReprocessingRequest(fiscalizationReprocessDTOList);

                log.LogMethodExit(dtoList);
                return dtoList;
            });
        }
    }
}
