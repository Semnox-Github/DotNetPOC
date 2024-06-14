/********************************************************************************************
 * Project Name - RedemptionUtils
 * Description  - LocalRedemptionCurrencyUseCase class to get the data  from local DB 
 * 
 *   
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0          05-Nov-2020      Vikas Dwivedi             Created : POS UI Redesign with REST API
 2.110.1      17-Feb-2021      Mushahid Faizan           Modified - Web Inventory Phase 2 changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Redemption
{
    public class LocalRedemptionCurrencyUseCases : LocalUseCases, IRedemptionCurrencyUseCases
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public LocalRedemptionCurrencyUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public async Task<RedemptionCurrencyContainerDTOCollection> GetRedemptionCurrencyContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            return await Task<RedemptionCurrencyContainerDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(siteId, hash, rebuildCache);
                if (rebuildCache)
                {
                    RedemptionCurrencyContainerList.Rebuild(siteId);
                }
                List<RedemptionCurrencyContainerDTO> currencyContainerList = RedemptionCurrencyContainerList.GetRedemptionCurrencyContainerDTOList(siteId);
                RedemptionCurrencyContainerDTOCollection result = new RedemptionCurrencyContainerDTOCollection(currencyContainerList);
                if (hash == result.Hash)
                {
                    log.LogMethodExit(null, "No changes to the cache");
                    return null;
                }
                log.LogMethodExit(result);
                return result;
            });
        }

        public async Task<List<RedemptionCurrencyDTO>> GetRedemptionCurrencies(List<KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>> parameters,
            int currentPage = 0, int pageSize = 0, SqlTransaction sqlTransaction = null)
        {
            return await Task<List<RedemptionCurrencyDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(parameters);
                RedemptionCurrencyList redemptionCurrencyList = new RedemptionCurrencyList(executionContext);
                List<RedemptionCurrencyDTO> redemptionCurrencyDTOList = redemptionCurrencyList.GetAllRedemptionCurrency(parameters, currentPage, pageSize, sqlTransaction);
                log.LogMethodExit(redemptionCurrencyDTOList);
                return redemptionCurrencyDTOList;
            });
        }

        public async Task<int> GetRedemptionCurrencyCount(List<KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>> parameters)
        {
            return await Task<int>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(parameters);
                RedemptionCurrencyList redemptionCurrencyList = new RedemptionCurrencyList(executionContext);
                int redemptionCount = redemptionCurrencyList.GetRedemptionCurrenciesCount(parameters);
                log.LogMethodExit(redemptionCount);
                return redemptionCount;
            });
        }

        public async Task<string> SaveRedemptionCurrencies(List<RedemptionCurrencyDTO> redemptionCurrencyDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = "Success";
                log.LogMethodEntry(redemptionCurrencyDTOList);
                if (redemptionCurrencyDTOList == null)
                {
                    throw new ValidationException("redemptionCurrencyDTOList is Empty");
                }

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (RedemptionCurrencyDTO redemptionCurrencyDTO in redemptionCurrencyDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            RedemptionCurrency redemptionCurrency = new RedemptionCurrency(executionContext, redemptionCurrencyDTO);
                            redemptionCurrency.Save(parafaitDBTrx.SQLTrx);
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw ;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw ;
                        }
                    }
                }
                log.LogMethodExit(result);
                return result;
            });
        }

      
    }
}
