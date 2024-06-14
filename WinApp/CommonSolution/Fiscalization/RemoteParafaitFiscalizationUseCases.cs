/********************************************************************************************
* Project Name - Fiscalization
* Description  - Class for RemoteParafaitFiscalizationUseCases 
* 
**************
**Version Log
**************
*Version     Date              Modified By        Remarks          
*********************************************************************************************
*2.155.1       13-Aug-2023       Guru S A           Created for Chile fiscaliation
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
    /// RemoteParafaitFiscalizationUseCases
    /// </summary>
    public class RemoteParafaitFiscalizationUseCases: RemoteUseCases, IParafaitFiscalizationUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string FISCALIZATION_GET_URL = "api/Transaction/Fiscalization";
        private const string FISC_CONC_REQ_POST_URL = "api/ConcurrentRequest/Fiscalization";
        private const string GET_PENDING_TRANSACTIONS_COUNT = "api/Transaction/Fiscalization/GetPendingTransactionsCount";
        private Utilities utilities;
        /// <summary>
        /// RemoteParafaitFiscalizationUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="utilities"></param>
        public RemoteParafaitFiscalizationUseCases(ExecutionContext executionContext, Utilities utilities)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.utilities = utilities;
            log.LogMethodExit();
        }
        /// <summary>
        /// GetPendingTransactions
        /// </summary> 
        /// <returns></returns>
        public async Task<List<FiscalizationPendingTransactionDTO>> GetPendingTransactions(ParafaitFiscalizationNames parafaitFiscalizationName, 
            List<KeyValuePair<FiscalizationPendingTransactionDTO.SearchParameters, string>>
                          parameters, int pageNumber = 0, int pageSize = 50, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parafaitFiscalizationName, parameters, pageNumber, pageSize, sqlTransaction);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();            
            searchParameterList.Add(new KeyValuePair<string, string>("fiscalization".ToString(), parafaitFiscalizationName.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("pageNumber".ToString(), pageNumber.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("pageSize".ToString(), pageSize.ToString()));
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                List<FiscalizationPendingTransactionDTO> result = await Get<List<FiscalizationPendingTransactionDTO>>(FISCALIZATION_GET_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }        
        /// <summary>
        /// GetPendingTransactionCount
        /// </summary> 
        /// <returns></returns>
        public async Task<int> GetPendingTransactionCount(ParafaitFiscalizationNames parafaitFiscalizationName, 
            List<KeyValuePair<FiscalizationPendingTransactionDTO.SearchParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parafaitFiscalizationName, searchParameters, sqlTransaction);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("fiscalization".ToString(), parafaitFiscalizationName.ToString()));
            if (searchParameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(searchParameters));
            }
            try
            {
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                int result = await Get<int>(GET_PENDING_TRANSACTIONS_COUNT, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }
        /// <summary>
        /// PostFiscalizationReprocessingRequest
        /// </summary>
        /// <param name="fiscalizationReprocessDTOList"></param>
        /// <returns></returns>
        public async Task<List<FiscalizationReprocessDTO>> PostFiscalizationReprocessingRequest(ParafaitFiscalizationNames parafaitFiscalizationName, List<FiscalizationReprocessDTO> fiscalizationReprocessDTOList)
        {
            log.LogMethodEntry(parafaitFiscalizationName, fiscalizationReprocessDTOList);
            try
            {
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                List<FiscalizationReprocessDTO> result = await Post<List<FiscalizationReprocessDTO>>(FISC_CONC_REQ_POST_URL, fiscalizationReprocessDTOList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<FiscalizationPendingTransactionDTO.SearchParameters, string>> searchParams)
        {
            log.LogMethodEntry(searchParams);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<FiscalizationPendingTransactionDTO.SearchParameters, string> searchParameter in searchParams)
            {
                switch (searchParameter.Key)
                {
                    case FiscalizationPendingTransactionDTO.SearchParameters.TRX_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("transactionId".ToString(), searchParameter.Value));
                        }
                        break;
                    case FiscalizationPendingTransactionDTO.SearchParameters.TRX_FROM_DATE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("transactionFromDate".ToString(), searchParameter.Value));
                        }
                        break;
                    case FiscalizationPendingTransactionDTO.SearchParameters.TRX_TO_DATE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("tranasctionToDate".ToString(), searchParameter.Value));
                        }
                        break;
                    case FiscalizationPendingTransactionDTO.SearchParameters.IGNORE_WIP_TRX:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("ignoreWIPTransactions".ToString(), searchParameter.Value));
                        }
                        break;

                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
    }
}
