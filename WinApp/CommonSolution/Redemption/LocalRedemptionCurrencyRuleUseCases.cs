/********************************************************************************************
 * Project Name - POS 
 * Description  - LocalRedemptionCurrencyRuleUseCases class to get the data  from local DB 
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0          07-Nov-2020      Vikas Dwivedi             Created : POS UI Redesign with REST API
 2.110.1      14-Feb-2021      Mushahid Faizan           Modified : Web Inventory Phase 2 changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Redemption
{
    public class LocalRedemptionCurrencyRuleUseCases : LocalUseCases, IRedemptionCurrencyRuleUseCases
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public LocalRedemptionCurrencyRuleUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<RedemptionCurrencyRuleContainerDTOCollection> GetRedemptionCurrencyRuleContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            return await Task<RedemptionCurrencyRuleContainerDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(siteId, hash, rebuildCache);
                if (rebuildCache)
                {
                    RedemptionCurrencyRuleContainerList.Rebuild(siteId);
                }
                RedemptionCurrencyRuleContainerDTOCollection result = RedemptionCurrencyRuleContainerList.GetRedemptionCurrencyRuleContainerDTOCollection(siteId);
                if (hash == result.Hash)
                {
                    log.LogMethodExit(null, "No changes to the cache");
                    return null;
                }
                log.LogMethodExit(result);
                return result;
            });
        }

        public async Task<List<RedemptionCurrencyRuleDTO>> GetRedemptionCurrencyRules(List<KeyValuePair<RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters, string>> parameters,
            bool loadChildRecords = false, bool activeChildRecords = true, int currentPage = 0, int pageSize = 0, SqlTransaction sqlTransaction = null)
        {
            return await Task<List<RedemptionCurrencyRuleDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(parameters);
                RedemptionCurrencyRuleListBL redemptionCurrencyRuleListBL = new RedemptionCurrencyRuleListBL(executionContext);
                List<RedemptionCurrencyRuleDTO> redemptionCurrencyRuleDTOList = redemptionCurrencyRuleListBL.GetAllRedemptionCurrencyRuleList(parameters, loadChildRecords, activeChildRecords, currentPage, pageSize, sqlTransaction);
                log.LogMethodExit(redemptionCurrencyRuleDTOList);
                return redemptionCurrencyRuleDTOList;
            });
        }

        public async Task<int> GetRedemptionCurrencyRuleCount(List<KeyValuePair<RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters, string>> parameters,
             SqlTransaction sqlTransaction = null)
        {
            return await Task<int>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(parameters);
                RedemptionCurrencyRuleListBL redemptionCurrencyRuleListBL = new RedemptionCurrencyRuleListBL(executionContext);
                int redemptionCurrencyRuleCount = redemptionCurrencyRuleListBL.GetCurrencyRulesCount(parameters);
                log.LogMethodExit(redemptionCurrencyRuleCount);
                return redemptionCurrencyRuleCount;
            });
        }

        public async Task<string> SaveRedemptionCurrencyRules(List<RedemptionCurrencyRuleDTO> redemptionCurrencyRuleDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                log.LogMethodEntry(redemptionCurrencyRuleDTOList);
                if (redemptionCurrencyRuleDTOList == null)
                {
                    throw new ValidationException("redemptionCurrencyRuleDTOList is Empty");
                }

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (RedemptionCurrencyRuleDTO redemptionCurrencyRuleDTO in redemptionCurrencyRuleDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            RedemptionCurrencyRuleBL redemptionCurrencyRuleBL = new RedemptionCurrencyRuleBL(executionContext, redemptionCurrencyRuleDTO);
                            redemptionCurrencyRuleBL.Save(parafaitDBTrx.SQLTrx);
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
                result = "Success";
                log.LogMethodExit(result);
                return result;
            });
        }


    }
}
